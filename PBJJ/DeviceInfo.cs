using System.Linq;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace PBJJ
{
    public static class DeviceInfo
    {
        public static string GetCurrentIpv4Address()
        {
            // https://sandervandevelde.wordpress.com/2015/08/28/getting-ip-address-in-win-10-iot-core/

            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp?.NetworkAdapter?.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;

                var hostnames = NetworkInformation.GetHostNames();

                foreach (var hn in hostnames)
                {
                    if (hn.IPInformation?.NetworkAdapter?.NetworkAdapterId != null
                        && hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId && hn.Type == HostNameType.Ipv4)
                    {
                        return hn.CanonicalName;
                    }
                }
            }

            return "?";
        }

        public static string GetDeviceName()
        {
            // https://github.com/ms-iot/samples/blob/develop/IoTCoreDefaultApp/IoTCoreDefaultApp/Presenters/DeviceInfoPresenter.cs

            var hostname = NetworkInformation.GetHostNames()
                .FirstOrDefault(x => x.Type == HostNameType.DomainName);
            return hostname?.CanonicalName ?? "?";
        }
    }
}
