using Queue.Services.DTO;
using System;

namespace Queue.Terminal.Extensions
{
    public static class LifeSituationExtensions
    {
        private const string DefaultLifeSituationColor = "Blue";

        public static string GetColor(this LifeSituation situation)
        {
            return String.IsNullOrEmpty(situation.Color) ?
                                        situation.LifeSituationGroup == null ?
                                                                DefaultLifeSituationColor :
                                                                situation.LifeSituationGroup.Color :
                                        situation.Color;
        }
    }
}