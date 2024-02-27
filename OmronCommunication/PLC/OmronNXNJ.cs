using OmronCommunication.DataTypes;
using OmronCommunication.Network;
using OmronCommunication.Profinet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OmronCommunication.PLC
{
    /// <summary>
    /// OMRON NX/NJ 系列PLC，遵循FINS/UDP协议
    /// </summary>
    public class OmronNXNJ:NetworkDeviceBase, IPLC
    {
        public OmronNXNJ(string ipaddress):base(ipaddress)
        {
            finsCommand.DA1 = Convert.ToByte(ipaddress.Substring(ipaddress.LastIndexOf(".") + 1));
        }

        public OmronNXNJ(string ipaddress, int port):base(ipaddress, port)
        {
            finsCommand.DA1 = Convert.ToByte(ipaddress.Substring(ipaddress.LastIndexOf(".") + 1));
        }
        public OmronNXNJ(string ipaddress, int port, string name):base(ipaddress, port, name)
        {
            finsCommand.DA1 = Convert.ToByte(ipaddress.Substring(ipaddress.LastIndexOf(".") + 1));
        }

        private FinsCommand? finsCommand = new FinsCommand();

        #region Connect

        protected override OperateResult Connect()
        {
            return base.Connect();
        }

        public OperateResult Connect()
        {

        }


        #endregion


        #region Read & Write
        public bool[] ReadBool(string address, ushort length)
        {
            throw new NotImplementedException();
        }

        public bool ReadBool(string address)
        {
            throw new NotImplementedException();
        }

        public float ReadFoalt(string address)
        {
            throw new NotImplementedException();
        }

        public int ReadInt(string address)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteBool(string address, bool value)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteFoalt(string address, float value)
        {
            throw new NotImplementedException();
        }

        public OperateResult WriteInt(string address, int value)
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> Read(string address, ushort length)
        {
            //BuildReadCommand
            var result = finsCommand.BuildReadFinsCommand(address, length,false);

            //ReadFromServer


            //DataAnalysis
            throw new NotImplementedException();
        }

        public OperateResult<bool> ReadBool(string address, ushort length)
        {
            //BuildReadCommand
            var result = finsCommand.BuildReadFinsCommand(address, length, false);

            //ReadFromServer


            //DataAnalysis
            throw new NotImplementedException();
        }

        #endregion 
    }
}
