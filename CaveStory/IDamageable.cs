using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public interface IDamageable
    {
        GameUnit CenterX { get; }
        GameUnit CenterY { get; }
        DamageText DamageText { get; }
    }
}
