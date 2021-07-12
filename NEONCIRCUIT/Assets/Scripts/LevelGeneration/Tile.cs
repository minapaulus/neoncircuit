using System.Collections;
using System.Collections.Generic;

public class Tile
{
    public bool North {get;set;}
    public bool East {get;set;}
    public bool South {get;set;}
    public bool West {get;set;}

    public Tile(bool n, bool e, bool s, bool w)
    {
        North = n;
        East = e;
        South = s;
        West = w;
    }
}
