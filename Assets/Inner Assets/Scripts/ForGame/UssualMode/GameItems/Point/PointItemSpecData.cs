using System.Collections.Generic;

public class PointItemSpecData : MainSpecData
{
    // Использование заранее подготовленого варианта спец данных
    public MovingByRangeSpecData movingByRangeSpecData;

    public override Dictionary<string, string> GetSpecDataValues()
    {
        var dict = new Dictionary<string, string>();

        movingByRangeSpecData.GetData(ref dict);
        return dict;
    }

    public override void SetSpecDataValues(Dictionary<string, string> specData)
    {
        movingByRangeSpecData.SetData(specData);
        return;
    }

}
