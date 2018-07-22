public class MathUtility
{
    public static float WrapValue(float input, float min, float max)
    {
        if (input > max)
        {
            input = min;
        }
        else if (input < min)
        {
            input = max;
        }

        return input;
    }
}
