public static class NumberFormatter
{
    private static string[] _nomes = new[] { "K", "M", "B", "T" };

    public static string Format(double num)
    {
        if (num >= 1000000000000)
        {
            return (num / 1000000000000.0).ToString("0.##") + _nomes[3];
        }
        else if (num >= 1000000000)
        {
            return (num / 1000000000.0).ToString("0.##") + _nomes[2];
        }
        else if (num >= 1000000)
        {
            return (num / 1000000.0).ToString("0.##") + _nomes[1];
        }
        else if (num >= 1000)
        {
            return (num / 1000.0).ToString("0.##") + _nomes[0]; 
        }
        else
        {
            num = (int)num;
            return num.ToString(); 
        }
    }
}
