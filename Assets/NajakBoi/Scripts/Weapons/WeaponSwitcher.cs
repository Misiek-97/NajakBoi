using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NajakBoi.Scripts.Weapons
{
    public class WeaponSwitcher : MonoBehaviour
    {

        public List<Weapon> weapons;

        public Weapon currentWeapon;
        
        public GameManager gm => GameManager.Instance;
        private PlayerController _player;
        
        // Start is called before the first frame update
        void Start()
        {
            _player = GetComponentInParent<PlayerController>();
            foreach (var weapon in transform.GetComponentsInChildren<Weapon>())
            {
                weapons.Add(weapon);

                if (weapon.weaponType != WeaponType.Pistol)
                {
                    weapon.gameObject.SetActive(false);
                }
                else
                {
                    currentWeapon = weapon;
                    _player.controller.AnimatorSetWeaponType((int)currentWeapon.weaponType);
                }
            }
        }

        private void SelectWeapon(WeaponType type)
        {
            currentWeapon.gameObject.SetActive(false);
            var weapon = weapons.First(x => x.weaponType == type);
            weapon.gameObject.SetActive(true);
            currentWeapon = weapon;
            _player.controller.AnimatorSetWeaponType((int)currentWeapon.weaponType);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && gm.IsMyTurn(_player.playerId))
            {
                switch (currentWeapon.weaponType)
                {
                    case WeaponType.Pistol:
                        SelectWeapon(WeaponType.Launcher);
                        break;
                    case WeaponType.Rifle:
                        break;
                    case WeaponType.Sniper:
                        break;
                    case WeaponType.Launcher:
                        SelectWeapon(WeaponType.Pistol);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
