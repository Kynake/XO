using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
  private static List<GameObject> _menus;

  private static GameObject _background = null;

  private void Awake() {
    _menus = _menus ?? new List<GameObject>(GameObject.FindGameObjectsWithTag("Menu"));
    _background = _background ?? GameObject.FindGameObjectWithTag("Background");

    Board.OnGameStarted += hideAllMenus;

    _background.SetActive(true);
  }

  public static void toggleMenu(GameObject activeMenu) {
    if(activeMenu.tag != "Menu") {
      return;
    }

    _menus.ForEach(menu => menu.SetActive(menu == activeMenu));
    _background.SetActive(true);
  }

  public static void hideAllMenus() {
    _menus.ForEach(menu => menu.SetActive(false));
    _background.SetActive(false);
  }
}
