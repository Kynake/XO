using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
  private static List<GameObject> _menus;

  private void Awake() {
    _menus = _menus ?? new List<GameObject>(GameObject.FindGameObjectsWithTag("Menu"));
  }

  public static void toggleMenu(GameObject activeMenu) {
    if(activeMenu.tag != "Menu") {
      return;
    }

    _menus.ForEach(menu => menu.SetActive(menu == activeMenu));
  }
}
