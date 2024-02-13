using System;
using System.Collections.Generic;
using System.Linq;
using NajakBoi.Scripts.Player;
using UnityEngine;

namespace NajakBoi.Scripts.Weapons
{
    public class WeaponSwitcher : MonoBehaviour
    {

        public List<Weapon> weapons;

        public Weapon currentWeapon;
        
        public GameManager gm => GameManager.Instance;
        private NajakBoiController _najakBoi;
        
        // Start is called before the first frame update
        void Start()
        {
            _najakBoi = GetComponentInParent<NajakBoiController>();
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
                }
            }
        }

        private void SelectWeapon(WeaponType type)
        {
            currentWeapon.gameObject.SetActive(false);
            var weapon = weapons.First(x => x.weaponType == type);
            weapon.gameObject.SetActive(true);
            currentWeapon = weapon;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && gm.IsMyTurn(_najakBoi.playerId))
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
