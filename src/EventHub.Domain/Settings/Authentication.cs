﻿namespace EventHub.Domain.Settings;

public class Authentication
{
    public ProviderOptions Google { get; set; }

    public ProviderOptions Facebook { get; set; }
}