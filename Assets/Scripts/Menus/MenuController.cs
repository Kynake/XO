using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
  [SerializeField]
  private GameObject _background = null;

  private static List<GameObject> _menus;

  private void Awake() {
    _menus = _menus ?? new List<GameObject>(GameObject.FindGameObjectsWithTag("Menu"));

    _background.SetActive(true);

    Board.OnGameStarted += () => {
      _background.SetActive(false);
      hideAllMenus();
    };

    Board.OnGameEnded += (Symbol _) => _background.SetActive(true);
    Board.OnGameInterrupted += () => _background.SetActive(true);

  }

  public static void toggleMenu(GameObject activeMenu) {
    if(activeMenu.tag != "Menu") {
      return;
    }

    _menus.ForEach(menu => menu.SetActive(menu == activeMenu));
  }

  public static void hideAllMenus() => _menus.ForEach(menu => menu.SetActive(false));
}
