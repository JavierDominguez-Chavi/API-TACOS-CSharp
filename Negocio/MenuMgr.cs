namespace TACOS.Negocio;
using System.Collections.Generic;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

public class MenuMgr : ManagerBase, IMenuMgt
{
    public MenuMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }
}