using MLAPI;

public class NetBehaviour : NetworkBehaviour {
  public enum NetAppType {
    None            = 0b00, // Neither Server nor Client
    Host            = 0b11, // Both Server and Client
    DedicatedServer = 0b10, // Server only
    ThinClient      = 0b01, // Client only

    Server = DedicatedServer | Host, // Definitely Server, Maybe Client
    Client = ThinClient      | Host, // Definitely Client, Maybe Server
  }

  protected bool IsNone            { get => !(IsServer ||  IsClient); }
  protected bool IsDedicatedServer { get =>   IsServer && !IsClient;  }
  protected bool IsThinClient      { get =>  !IsServer &&  IsClient;  }

  protected NetAppType NetworkAppType {
    get {
      if(IsHost) {
        return NetAppType.Host;
      }

      if(IsServer) {
        return NetAppType.DedicatedServer;
      }

      if(IsClient) {
        return NetAppType.ThinClient;
      }

      return NetAppType.None;
    }
  }
}