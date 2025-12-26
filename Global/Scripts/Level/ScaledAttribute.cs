using System;

public class ScaledAttribute
{
    private double value;
    private Func<double, double> scale;

    public ScaledAttribute(Func<double, double> scale)
    {
        this.scale = scale;
        value = scale.Invoke(0);
    }

    public void Update(double x) => value = scale.Invoke(x);

    public double Value() => value;
}