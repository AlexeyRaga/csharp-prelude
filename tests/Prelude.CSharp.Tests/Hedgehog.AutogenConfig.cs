using Hedgehog;
using Hedgehog.Linq;
using Prelude.Hedgehog;

namespace Prelude.CSharp.Tests;

public static class GeneratorsConfig
{
    public static AutoGenConfig Config =>
        GenX.defaults.WithGenerators<PreludeGen>();
}
