using System;

namespace RIM_CLI
{
    public struct Image
    {
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public bool IsEmpty => Data == null || Data.Length == 0;
        public bool HasJpegHeader => HasJpegHeaderInternal();
        
        private bool HasJpegHeaderInternal()
        {
            if (Data.Length < 4) return false;
            
            var soi = BitConverter.ToUInt16(Data, 0);
            var marker = BitConverter.ToUInt16(Data, 2);

            return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
        }
    }
}