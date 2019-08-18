﻿namespace Kompression.LempelZiv.PriceCalculators
{
    public interface IPriceCalculator
    {
        int CalculateLiteralLength(byte value);
        int CalculateMatchLength(LzMatch match);
    }
}