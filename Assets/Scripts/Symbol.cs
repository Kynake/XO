public enum Symbol {
    None = 0,
    Cross = 1,
    Nought = -1
}

public static class SymbolExtensions {
    public static Symbol other(this Symbol s) => (Symbol) (-(int) s);
}