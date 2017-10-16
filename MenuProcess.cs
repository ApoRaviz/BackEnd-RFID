using System;
using System.Collections.Generic;
using WMS.Master;
using WMS.Master.Menu;

public static class MenuProcess
{
	public static MenuProcess()
	{
	}

    public static List<MenuDto> GetParentMenu(List<MenuDto> menu , List<List<MenuDto>> allMenu)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            FindParent(menu[i], allmenu);
        }
    }

    public void FindParent(MenuDto mother, List<List<MenuDto>> allData)
    {
        List<MenuDto> temp;
        for (int i = 0; i < allData.Count(); i++)
        {
            temp = allData[i];
            if (temp[0].MenuParentID == mother.MenuIDSys)
            {
                mother.ParentMenu = temp;
            }
        }
        if (mother.ParentMenu != null)
        {
            for (int i = 0; i < mother.ParentMenu.Count(); i++)
            {
                FindParent(mother.ParentMenu[i], allData);
            }
        }
    }

}
