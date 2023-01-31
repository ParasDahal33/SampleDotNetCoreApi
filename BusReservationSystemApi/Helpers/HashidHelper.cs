using HashidsNet;

namespace BusReservationSystemApi.Helpers
{

    public interface IHashIdHelper
    {
        public int HashIdDecode(string hashId);
        public string HashIdEncode(int id);
    }

    public class HashIdHelper : IHashIdHelper
    {
        private readonly IHashids _hashids;
        public HashIdHelper(IHashids hashids)
        {
            _hashids = hashids;
        }
        public int HashIdDecode(string hashId)
        {
            var rawId = _hashids.Decode(hashId);
            if (rawId.Length == 0)
            {
                throw new KeyNotFoundException("Id not found");
            }
            return int.Parse(rawId[0].ToString());
        }
        public string HashIdEncode(int id)
        {
            string hashId;
            try
            {
                hashId = _hashids.Encode(id);
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException("Unable to hash " + ex);
            }
            return hashId;
        }
    }

}
