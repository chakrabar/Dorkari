namespace Dorkari.Samples.Cmd.Tests
{
    interface IEmptyInterface //builds FINE - interface can be empty
    { }

    class EmptyCLass //builds FINE - class can be empty
    { }

    static class EmptyStaticClass //builds FINE - static class can also be empty
    { }

    abstract class EmptyAbstractClass //builds FINE - abstract class can also be empty
    { }

    sealed class EmptySealedClass //builds FINE - sealed class can also be empty
    { }

    struct EmptyStruct //builds FINE - struct can be empty
    { }
}
