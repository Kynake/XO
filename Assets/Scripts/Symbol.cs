public enum Symbol {
    None,
    Cross,
    Nought
}

public class SymbolOperations {
    public static Symbol other(Symbol s) {
        switch(s) {
            case Symbol.Cross:
                return Symbol.Nought;

            case Symbol.Nought:
                return Symbol.Cross;

            default:
                return Symbol.None;
        }
    }
}