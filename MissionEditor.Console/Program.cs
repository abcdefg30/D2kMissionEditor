using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using MissionEditor.FileReaderCore;

namespace MissionEditor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            var filePath = @args[0];
            var outputName = args.Length > 1 ? args[1] : "testDump.txt";

            var mission = FileReaderCore.FileReader.ParseMissionFile(filePath);
            FileReaderCore.FileReader.DumpDataToFile(mission, outputName);
        }

        static MissionTest ByteArrayToStructure(byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stuff = (MissionTest)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(MissionTest));
            handle.Free();
            return stuff;
        }

        object ByteArrayToStructure(byte[] bytearray, object structureObj, int position)
        {
            var length = Marshal.SizeOf(structureObj);
            var ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(bytearray, 0, ptr, length);
            structureObj = Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(bytearray, position), structureObj.GetType());
            Marshal.FreeHGlobal(ptr);
            return structureObj;
        }   
    }
}
