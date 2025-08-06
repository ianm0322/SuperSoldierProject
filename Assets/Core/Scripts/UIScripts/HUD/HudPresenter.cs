using Cysharp.Threading.Tasks;
using R3;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PrSuperSoldier.UI
{
    public class HudPresenter
    {
        private const string CrosshairHookableClass = "crosshair-hookable";
        private const string CrosshairNotHookableClass = "crosshair-nothookable";

        private readonly HudViewer _viewer;
        private readonly GameObject _playerGO;
        private readonly PlayerHookController _hookController;
        private readonly PlayerMeleeController _meleeController;

        private IDisposable _hookableStateChangedHandle;
        private IDisposable _hookAmmoChangedHandle;
        private IDisposable _timerUpdateHandle;
        private int _enabledHookAmmoCount = 0;

        public HudPresenter(HudViewer viewer, GameObject player)
        {
            this._viewer = viewer;
            _playerGO = player;
            _hookController = _playerGO.GetComponent<PlayerHookController>();
            _meleeController = _playerGO.GetComponent<PlayerMeleeController>();
        }

        public void OnEnabled()
        {
            // Crosshair ���� ��ƾ ����.
            _hookableStateChangedHandle = Observable.EveryValueChanged(this, (o) => _hookController.CanHookToTarget())
                .Subscribe(UpdateHookableState);

            // Ammo ���÷��� ��ƾ ����.
            _enabledHookAmmoCount = _hookController.MaxHookCount - _hookController.CurrentHookCount;
            _hookAmmoChangedHandle = Observable.EveryValueChanged(this, (o) => _hookController.CurrentHookCount)
                .Subscribe(currentHookCount =>
                {
                    _enabledHookAmmoCount = _hookController.MaxHookCount - currentHookCount;
                    UpdateHookAmmoDisplay();
                });

            // Ÿ�̸� ������Ʈ ��ƾ ����.
            _timerUpdateHandle = Observable.EveryValueChanged(this, (o) => PlaySceneManager.Instance.ElapsedTime)
                .Subscribe(elapsedTime =>
                {
                    _viewer.TimerText.text = TimeSpan.FromSeconds(elapsedTime).ToString(@"mm\:ss\.ff");
                });

            // ���� �� ��Ÿ�� ǥ�� �̺�Ʈ �Ҵ�.
            _meleeController.OnMeleeExecute += async () =>
            {
                Debug.Log("Melee cooldown");
                var cooldownTime = _meleeController.CooldownTime;
                float timer = 0;
                while (timer < cooldownTime)
                {
                    await UniTask.NextFrame();
                    timer += Time.deltaTime;
                    float ratio = timer / cooldownTime;
                    _viewer.MeleeCooldownBar.style.backgroundSize = new BackgroundSize(new Length(100, LengthUnit.Percent), new Length(100 * ratio, LengthUnit.Percent));
                }
                _viewer.MeleeCooldownBar.style.backgroundSize = new BackgroundSize(new Length(100, LengthUnit.Percent), new Length(100, LengthUnit.Percent));
            };
        }

        public void OnDisabled()
        {
            _hookableStateChangedHandle.Dispose();
            _hookAmmoChangedHandle.Dispose();
        }

        private void UpdateHookableState(bool hookable)
        {
            if (hookable)
            {
                _viewer.Crosshair.AddToClassList(CrosshairHookableClass);
                _viewer.Crosshair.RemoveFromClassList(CrosshairNotHookableClass);
            }
            else
            {
                _viewer.Crosshair.RemoveFromClassList(CrosshairHookableClass);
                _viewer.Crosshair.AddToClassList(CrosshairNotHookableClass);
            }
        }

        private void UpdateHookAmmoDisplay()
        {
            for (int i = 0; i < _viewer.AmmoDisplays.Count; ++i)
            {
                // �ִ� ���� �ʰ��� ź�� ǥ�ø� ���� ó��
                if (i >= _hookController.MaxHookCount)
                {
                    _viewer.AmmoDisplays[i].AddToClassList("ammo-hidden");
                }
                else
                {
                    // �ִ� ���� ������ ź�� ǥ�ø� ���� ����
                    _viewer.AmmoDisplays[i].RemoveFromClassList("ammo-hidden");

                    if (i < _enabledHookAmmoCount)
                    {
                        EnableHookAmmo(_viewer.AmmoDisplays[i]);
                    }
                    else
                    {
                        DisableHookAmmo(_viewer.AmmoDisplays[i]);
                    }
                }
            }
        }
        private void EnableHookAmmo(VisualElement ammoDisplay)
        {
            ammoDisplay.AddToClassList("ammo-enable");
            ammoDisplay.RemoveFromClassList("ammo-disable");
        }
        private void DisableHookAmmo(VisualElement ammoDisplay)
        {
            ammoDisplay.AddToClassList("ammo-disable");
            ammoDisplay.RemoveFromClassList("ammo-enable");
        }
    }
}