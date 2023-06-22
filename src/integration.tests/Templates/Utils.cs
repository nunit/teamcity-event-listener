[System.Serializable]
internal class UnloadingDomainUtil
{
    public static void Create()
    {
        var newDomain = System.AppDomain.CreateDomain(System.Guid.NewGuid().ToString(), System.AppDomain.CurrentDomain.Evidence);
        newDomain.CreateInstanceFrom(typeof(UnloadingDomainUtil).Assembly.Location, typeof(UnloadingDomainUtil).FullName);
    }

    public UnloadingDomainUtil()
    {
        new System.Threading.Thread(() => { while(true); }).Start();
    }   
}