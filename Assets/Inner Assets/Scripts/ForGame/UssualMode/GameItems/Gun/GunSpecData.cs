using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GunSpecData : MainSpecData
{
    public enum ShootVector
    {
        left, top, right, down
    }

    public float shootPeriodicityInSecond;
    public ShootVector shootVector;

    public override Dictionary<string, string> GetSpecDataValues()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(shootPeriodicityInSecond), shootPeriodicityInSecond.ToString());
        dict.Add(nameof(shootVector), shootVector.GetHashCode().ToString());

        return dict;
    }

    public override void SetSpecDataValues(Dictionary<string, string> specData)
    {
        shootPeriodicityInSecond = float.Parse(specData[nameof(shootPeriodicityInSecond)], CultureInfo.InvariantCulture);
        int shootVectorInt = int.Parse(specData[nameof(shootVector)]);
        switch (shootVectorInt)
        {
            case 0:
                shootVector = ShootVector.left;
                break;
            case 1:
                shootVector = ShootVector.top;
                break;
            case 2:
                shootVector = ShootVector.right;
                break;
            case 3:
                shootVector = ShootVector.down;
                break;
        }
        return;
    }
}
