/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.CommonLib.Message
{
    public enum MsgType
    {
        #region 交易报文

        /// <summary>
        /// 个人化
        /// </summary>
        TxnPersonal = 1001,

        /// <summary>
        /// 初始化
        /// </summary>
        TxnInitial = 1002,

        /// <summary>
        /// 预赋值
        /// </summary>
        TxnPrePayment = 1003,

        /// <summary>
        /// 票卡注销
        /// </summary>
        TxnTicketCancel = 1004,

        /// <summary>
        /// 重编码
        /// </summary>
        TxnTicketRecode = 1005,

        /// <summary>
        /// 预赋值单程票抵消
        /// </summary>
        TxnPrePaymentSjtOffset = 1006,

        /// <summary>
        /// 设备交易数据文件
        /// </summary>
        TxnDeviceTxnDataFile = 1007,

        /// <summary>
        /// 晚传交易数据文件
        /// </summary>
        TxnLaterTxnDataFile = 1008,

        /// <summary>
        /// 内卡黑名单捕获
        /// </summary>
        TxnInnerBlackListCapture = 1009,

        /// <summary>
        /// 外卡黑名单捕获
        /// </summary>
        TxnOuterBlackListCapture = 1010,

        /// <summary>
        /// Tvm购买单程票
        /// </summary>
        TxnTvmSjtSale = 1201,

        /// <summary>
        /// Tvm上SVT充值
        /// </summary>
        TxnTvmSvtCharge = 1202,

        /// <summary>
        /// Tvm上SVT购买单程票
        /// </summary>
        TxnTvmSvtBuyTicket = 1203,

        /// <summary>
        /// Tvm上SVT充值(外卡)
        /// </summary>
        TxnTvmOuterSvtCharge = 1204,

        /// <summary>
        /// Tvm上SVT充值(外卡)
        /// </summary>
        TxnTvmOuterSvtBuyTicket = 1205,

        /// <summary>
        /// Bom租卡(单程票发售)
        /// </summary>
        TxnBomSjtSale = 1301,

        /// <summary>
        /// Bom票卡充值
        /// </summary>
        TxnBomTicketCharge = 1302,

        /// <summary>
        /// Bom票卡更新
        /// </summary>
        TxnBomTicketUpdate = 1303,

        /// <summary>
        /// Bom即时退款
        /// </summary>
        TxnBomInstantRefund = 1304,

        /// <summary>
        /// Bom上Svt扣款
        /// </summary>
        TxnBomSvtWithhold = 1305,

        /// <summary>
        /// Bom处优惠使用
        /// </summary>
        TxnBomPreferentialUse = 1306,

        /// <summary>
        /// Bom上发行出站票
        /// </summary>
        TxnBomExitTicketSale = 1307,

        /// <summary>
        /// Bom票卡解锁
        /// </summary>
        TxnBomTicketUnlock = 1308,

        /// <summary>
        /// Bom上储值票充值(外卡)
        /// </summary>
        TxnBomOuterSvtCharge = 1309,

        /// <summary>
        /// Bom上Svt更新(外卡)
        /// </summary>
        TxnBomOuterSvtUpdate = 1310,

        /// <summary>
        /// Bom上Svt购买单程票(外卡)
        /// </summary>
        TxnBomOuterSvtBuyTicket = 1311,

        /// <summary>
        /// Bom上Svt发售(外卡)
        /// </summary>
        TxnBomOuterSvtSale = 1312,

        TxnBomAdmin = 1313,

        /// <summary>
        /// AGM进闸
        /// </summary>
        TxnAgmYptEntry = 1401,

        /// <summary>
        /// AGM出闸
        /// </summary>
        TxnAgmYptExit = 1402,

        /// <summary>
        /// AGM进闸(外卡)
        /// </summary>
        TxnAgmYktEntry = 1403,

        /// <summary>
        /// AGM出闸(外卡)
        /// </summary>
        TxnAgmYktExit = 1404,

        /// <summary>
        /// Bom非即时退款
        /// </summary>
        TxnBomNonInstantRefund = 1314,

        /// <summary>
        /// 记名卡挂失
        /// </summary>
        TxnBomNameCardLoss = 1315,

        /// <summary>
        /// Bom团体票发售
        /// </summary>
        TxnBomGroupTicketSale = 1316,

        /// <summary>
        /// Bom积分兑换
        /// </summary>
        TxnBomIntegration = 1317,

        /// <summary>
        /// Bom特殊票卡申领
        /// </summary>
        TxnBomSpecialTicketRequest = 1318,

        #endregion 交易报文

        #region 审计数据

        /// <summary>
        /// 寄存器数据
        /// </summary>
        AuditRegisterData = 2821,

        /// <summary>
        /// 交易汇总数据
        /// </summary>
        AuditTxnSummaryData = 2901,

        /// <summary>
        /// 补款通知
        /// </summary>
        AuditReplenishmentNotify = 2902,

        /// <summary>
        /// bom班次审计数据
        /// </summary>
        AuditBomShiftData = 2319,

        /// <summary>
        /// agm审计数据
        /// </summary>
        AuditAgmTxnAuditData = 2405,

        /// <summary>
        /// tvm审计数据
        /// </summary>
        AuditTvmTxnAuditData = 2206,

        /// <summary>
        /// Bom操作员班次审计数据
        /// </summary>
        AuditBomOperatorShiftData = 2320,

        #endregion 审计数据

        #region 命令回复数据

        /// <summary>
        /// 参数版本请求应答
        /// </summary>
        ResponePrmVersion = 3036,

        /// <summary>
        /// 软件版本请求应答
        /// </summary>
        ResponeSoftVersion = 3037,

        /// <summary>
        /// 纸币钱箱数量请求应答
        /// </summary>
        ResponeNoteQtyQuery = 3040,

        /// <summary>
        /// 硬币钱箱数量请求应答
        /// </summary>
        ResponeCoinQtyQuery = 3041,

        /// <summary>
        /// 车票数量请求应答
        /// </summary>
        ResponeTicketQtyQuery = 3042,

        #endregion 命令回复数据

        /// <summary>
        /// 设备整机/模块状态
        /// </summary>
        DeviceStatus = 3035,

        #region 线路自定义报文
        DeviceOnlineStat = 8200,

        PasswordChange = 8681,

        CmdContrlCmdExtend = 8900,

        TicketingTicketStockReport = 8901,
        TicketingTicketStoreChange = 8902,
        TicketingTicketBoxOperate = 8903,
        TicketingCashBoxOperate = 8904,
        TicketingCashStoreChange = 8905,
        TicketingStockTaking = 8906,
        TicketingScSettle = 8907,

        #region LC票务相关
        TicketApply = 8801,
        TicketApplyRespone = 8802,
        TicketAllot = 8803,
        TicketAllotRespone = 8804,
        TicketAllotBetweenStation = 8805,
        TicketAllotStatusChangeNotify = 8806,
        PreInitTicketSettle = 8807,
        PaperPresentReceiptAllot = 8808,
        TicketTurnIn = 8809,
        TicketTurnInRespone = 8810,
        AdjustTicketStock = 8811,
        StockInventory = 8812,
        TicketDispatchRepeal = 8813,
        StationTicketStockReport = 8814,
        ReplennishmentConfirm = 8815,
        #endregion LC票务相关
        #endregion 线路自定义报文

        #region 事物数据

        /// <summary>
        /// Agm票箱更换记录
        /// </summary>
        AgmTicketBoxChange = 4402,

        /// <summary>
        /// Agm票箱切换记录
        /// </summary>
        AgmTicketBoxStartStop = 4403,

        /// <summary>
        /// Tvm补票记录
        /// </summary>
        TvmTicketAdd = 4203,

        /// <summary>
        /// 钱箱更换记录
        /// </summary>
        TvmCashBoxChange = 4204,

        /// <summary>
        /// Tvm纸币补充记录
        /// </summary>
        TvmNoteBoxAdd = 4206,

        /// <summary>
        /// Tvm硬币补充记录
        /// </summary>
        TvmCoinBoxAdd = 4202,

        /// <summary>
        /// Tvm硬币补充记录
        /// </summary>
        TvmTicketBoxChange = 4207,

        /// <summary>
        /// 参数版本报告
        /// </summary>
        PrmVersionReport = 4082,

        /// <summary>
        /// 软件版本报告
        /// </summary>
        SoftVersionReport = 4083,

        /// <summary>
        /// 操作日志数据
        /// </summary>
        OperateLogData = 4084,

        /// <summary>
        /// Sam卡报告
        /// </summary>
        SamInfoReport = 4085,

        /// <summary>
        /// 特殊票卡报告
        /// </summary>
        SpecialTicketTraceReport = 4086,

        /// <summary>
        /// 行政现金交易记录
        /// </summary>
        BomAdminTxnData = 4030,

        #endregion 事物数据

        #region 参数数据

        /// <summary>
        /// 黑名单参数文件
        /// </summary>
        BlackListPrmFile = 5014,

        /// <summary>
        /// 计价方案参数文件
        /// </summary>
        PricePrmFile = 5015,

        /// <summary>
        /// 运营点参数文件
        /// </summary>
        StationPrmFile = 5016,

        /// <summary>
        /// 运营时间参数文件
        /// </summary>
        OperationTimePrmFile = 5017,

        /// <summary>
        /// 运营控制参数文件
        /// </summary>
        OperationControlPrmFile = 5018,

        /// <summary>
        /// 设备控制参数
        /// </summary>
        DeviceControlPrmFile = 5020,

        /// <summary>
        /// 设备权限参数
        /// </summary>
        DeviceprivilegePrmFile = 5021,

        /// <summary>
        /// 设备权限参数
        /// </summary>
        LineTimeControlPrmFile = 5024,

        /// <summary>
        /// 模式履历参数
        /// </summary>
        ModeResumePrmFile = 5025,

        /// <summary>
        /// 地图线网参数
        /// </summary>
        StationMapPrmFile = 5026,

        #endregion 参数数据

        #region 命令数据

        /// <summary>
        /// 设备服务模式控制
        /// </summary>
        CmdDeviceServiceModeControl = 6001,

        /// <summary>
        /// AGM闸门关闭
        /// </summary>
        CmdAgmGateClose = 6002,

        /// <summary>
        /// AGM闸门开启
        /// </summary>
        CmdAgmGateOpen = 6003,

        /// <summary>
        /// 系统降级模式控制
        /// </summary>
        CmdSystemDegradeMode = 6004,

        /// <summary>
        /// 系统运作模式
        /// </summary>
        CmdSystemOperateMode = 6005,

        /// <summary>
        /// 更换钱箱
        /// </summary>
        CmdCashBoxChange = 6008,

        /// <summary>
        /// 补充钱箱
        /// </summary>
        CmdCashBoxAdd = 6009,

        /// <summary>
        /// 更换票箱
        /// </summary>
        CmdTicketBoxChange = 6010,

        /// <summary>
        /// 补充票箱
        /// </summary>
        CmdTicketBoxAdd = 6011,

        /// <summary>
        /// 运营开始
        /// </summary>
        CmdRunningStart = 6012,

        /// <summary>
        /// 运营结束
        /// </summary>
        CmdRunningEnd = 6013,

        /// <summary>
        /// 设备状态请求
        /// </summary>
        CmdDeviceStatusRequest = 6015,

        /// <summary>
        /// 参数版本请求
        /// </summary>
        CmdPrmVersionRequest = 6016,

        /// <summary>
        /// 软件版本请求
        /// </summary>
        CmdSoftVersionRequest = 6017,

        /// <summary>
        /// 文件上传请求
        /// </summary>
        CmdFileUploadRequest = 6018,

        /// <summary>
        /// 模块状态请求
        /// </summary>
        CmdModuleStatusRequest = 6032,

        /// <summary>
        /// 票箱车票数量请求
        /// </summary>
        CmdTicketBoxQtyRequest = 6033,

        /// <summary>
        /// 纸币钱箱数量请求
        /// </summary>
        CmdNoteBoxQtyRequest = 6202,

        /// <summary>
        /// Coin钱箱数量请求
        /// </summary>
        CmdCoinBoxQtyRequest = 6203,

        /// <summary>
        /// TVM服务模式控制
        /// </summary>
        CmdTvmServiceModeControl = 6201,

        /// <summary>
        /// Agm用途模式转换
        /// </summary>
        CmdAgmUseModeChange = 6406,

        /// <summary>
        /// 时钟同步
        /// </summary>
        CmdClockSync = 6014,

        #endregion 命令数据

        #region 软件升级数据

        /// <summary>
        /// TVM软件
        /// </summary>
        SoftTvm = 7201,

        /// <summary>
        /// TVM软件
        /// </summary>
        SoftBom = 7301,

        /// <summary>
        /// Agm软件
        /// </summary>
        SoftAgm = 7401,

        /// <summary>
        /// Pca软件
        /// </summary>
        SoftPca = 7601,

        /// <summary>
        /// Bcm软件
        /// </summary>
        SoftBcm = 7701,

        /// <summary>
        /// 读写器软件
        /// </summary>
        SoftTp = 7901,

        #endregion 软件升级数据

        /// <summary>
        /// 模式变更通知
        /// </summary>
        AccModeChangeNotify = 9805,

        AccLineClockSyncQuery = 9807,
        AccLineClockSyncQueryRespone = 9808,
        AccParamVersionReport = 9809,
        AccClockSyncCommand = 9806,
        AccClockExceptionReport = 9811,
        DeviceRegisterMessage = 9870
    }

    public class BaseMessage
    {
        public MsgType msgType { get; set; }
        private int decodeIndex;
        private byte[] data;
        private bool isEncoded;
        protected List<byte> encodeBuf = new List<byte>();

        public int DecodeIndex
        {
            get { return decodeIndex; }
            set { decodeIndex = value; }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = Data; }
        }

        public BaseMessage()
        {
        }

        public BaseMessage(byte[] buf, int index = 0)
        {
            try
            {
                decodeIndex = index;
                data = buf;
                Decode();
                isEncoded = true;
            }
            catch (Exception ex)
            {
                throw new MsgParseFailException(decodeIndex, ex.ToString());
            }
        }

        public bool IsMd5Corrent()
        {
            //if (data.Length < 16)
            //{
            //    return false;
            //}

            //byte[] dataBytes = new byte[data.Length - 16];
            //Array.Copy(data, dataBytes, dataBytes.Length);

            //byte[] verifyMd5Bytes = new byte[16];
            //Array.Copy(data, data.Length - 16, verifyMd5Bytes, 0, 16);

            //return MessageHelper.VerifyMd5Bytes(dataBytes, verifyMd5Bytes);
            return true;
        }

        /// <summary>
        /// 解包
        /// </summary>
        public virtual void Decode()
        {
            decodeIndex = 0;
        }

        /// <summary>
        /// 生成数据包
        /// </summary>
        public virtual void Encode()
        {
        }

        /// <summary>
        /// 获取一单元字节数组(4字节)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isBigEndian">默认:大端</param>
        /// <returns></returns>
        private byte[] GetOneUnitBytes(int index, bool isBigEndian = true)
        {
            var bytes = new byte[4];

            if (isBigEndian)
            {
                bytes[0] = data[index + 3];
                bytes[1] = data[index + 2];
                bytes[2] = data[index + 1];
                bytes[3] = data[index];
            }
            else
            {
                bytes[0] = data[index];
                bytes[1] = data[index + 1];
                bytes[2] = data[index + 2];
                bytes[3] = data[index + 3];
            }

            return bytes;

            #region
            //             bytes[0] = data[index];
            //             bytes[1] = data[index + 1];
            //             bytes[2] = data[index + 2];
            //             bytes[3] = data[index + 3];
            //             if (isBigEndian)
            //             {
            //                 Array.Reverse(bytes);
            //             }
            //
            //             return bytes;
            #endregion
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetBytes()
        {
            if (!isEncoded)
            {
                Encode();
                isEncoded = true;
                data = encodeBuf.ToArray();
            }
            return data;
        }

        #region Decode

        /// <summary>
        /// 获取ASCII编码字符串
        /// </summary>
        /// <returns></returns>
        internal string GetNextString(int len)
        {
            // uint len = GetNextUInt();
            string str = Encoding.ASCII.GetString(data, decodeIndex, len);
            decodeIndex += len;
            return str;
        }

        internal string GetNextUnicodeString(int len)
        {
            // uint len = GetNextUInt();
            string str = Encoding.Unicode.GetString(data, decodeIndex, len);
            decodeIndex += len;
            return str;
        }

        /// <summary>
        /// 获取UTF8编码字符串
        /// </summary>
        /// <returns></returns>
        internal string GetNextUnicodeString()
        {
            //uint len = GetNextUInt();
            //string str = Encoding.UTF8.GetString(data, decodeIndex, (int)len);

            //if (len % 4 == 0)
            //{
            //    decodeIndex += (int)len;
            //}
            //else
            //{
            //    decodeIndex += ((int)len / 4 + 1) * 4;
            //}

            int len = Convert.ToInt32(GetNextString(3));

            string str = Encoding.Unicode.GetString(data, decodeIndex, len);
            decodeIndex += len;
            return str;
        }

        /// <summary>
        /// 获取byte数据 (对应U8_t)
        /// </summary>
        /// <returns></returns>
        internal byte GetNextByte()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            return bytes[0];
        }

        /// <summary>
        /// 获取ushort数据 (对应U16_t)
        /// </summary>
        /// <returns></returns>
        internal ushort GetNextUShort()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// 获取uint数据 (对应U32_t)
        /// </summary>
        /// <returns></returns>
        internal uint GetNextUInt()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// 获取sbyte数据 (S8_t 有符号)
        /// </summary>
        /// <returns></returns>
        internal sbyte GetNextSByte()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            byte b = bytes[0];

            if (b > 127)
                return (sbyte)(b - 256);
            else
                return (sbyte)b;
        }

        /// <summary>
        /// 获取short数据 (S16_t 有符号)
        /// </summary>
        /// <returns></returns>
        internal short GetNextShort()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// 获取int数据 (S32_t 有符号)
        /// </summary>
        /// <returns></returns>
        internal int GetNextInt()
        {
            var bytes = GetOneUnitBytes(decodeIndex);
            decodeIndex += 4;
            return BitConverter.ToInt32(bytes, 0);
        }

        internal uint GetUintByIndex(int index)
        {
            var bytes = GetOneUnitBytes(index);
            return BitConverter.ToUInt32(bytes, 0);
        }

        internal byte GetByteByIndex(int index)
        {
            var bytes = GetOneUnitBytes(index);
            return bytes[0];
        }

        #endregion Decode

        #region Encode

        internal byte[] AddString(string str, int len)
        {
            //if (value == string.Empty)
            //{
            //    AddInt(0);
            //}
            //else
            //{
            //    var bytes = Encoding.ASCII.GetBytes(value);
            //    int len = bytes.Length;

            //    AddInt(len);
            //    FillBytesWithUnits(bytes, false);
            //}

            byte[] buf = new byte[len];
            if (str != null)
            {
                byte[] tempBuf = Encoding.ASCII.GetBytes(str);
                Array.Copy(tempBuf, buf, tempBuf.Length < len ? tempBuf.Length : len);
            }
            return buf;
        }

        internal byte[] AddUnicodeString(string str, int len)
        {
            //if (value == string.Empty)
            //{
            //    AddInt(0);
            //}
            //else
            //{
            //    var bytes = Encoding.ASCII.GetBytes(value);
            //    int len = bytes.Length;

            //    AddInt(len);
            //    FillBytesWithUnits(bytes, false);
            //}

            byte[] buf = new byte[len];
            if (str != null)
            {
                byte[] tempBuf = Encoding.Unicode.GetBytes(str);
                Array.Copy(tempBuf, buf, tempBuf.Length < len ? tempBuf.Length : len);
            }
            return buf;
        }

        internal byte[] AddUnicodeString(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            int len = bytes.Length;

            encodeBuf.AddRange(Encoding.ASCII.GetBytes(len.ToString().PadLeft(3, '0')));
            //AddInt(len);
            // FillBytesWithUnits(bytes, false);
            return bytes;
        }

        internal void AddByte(byte value)
        {
            var bytes = new byte[4];
            bytes[3] = value;
            encodeBuf.AddRange(bytes);
        }

        internal void AddUShort(ushort value)
        {
            //value = MessageHelper.ReverseBytes(value);
            //var bytes = BitConverter.GetBytes(value);
            //FillBytesWithUnits(bytes);
        }

        internal void AddUInt(uint value)
        {
            //value = MessageHelper.ReverseBytes(value);
            //var bytes = BitConverter.GetBytes(value);
            //FillBytesWithUnits(bytes);
        }

        internal void AddSByte(sbyte value)
        {
            var bytes = new byte[4];
            bytes[3] = value < 0 ? (byte)(value + 256) : (byte)value;
            encodeBuf.AddRange(bytes);
        }

        internal void AddShort(short value)
        {
            //value = MessageHelper.ReverseBytes(value);
            //var bytes = BitConverter.GetBytes(value);
            //FillBytesWithUnits(bytes);
        }

        internal void AddInt(int value)
        {
            //value = MessageHelper.ReverseBytes(value);
            //var bytes = BitConverter.GetBytes(value);
            //FillBytesWithUnits(bytes);
        }

        private void FillBytesWithUnits(byte[] bytes, bool isPaddingLeft = true)
        {
            int paddingLen = 4 - bytes.Length % 4;
            if (paddingLen != 4)
            {
                if (isPaddingLeft)
                {
                    encodeBuf.AddRange(new byte[paddingLen]);
                    encodeBuf.AddRange(bytes);
                }
                else
                {
                    encodeBuf.AddRange(bytes);
                    encodeBuf.AddRange(new byte[paddingLen]);
                }
            }
            else
            {
                encodeBuf.AddRange(bytes);
            }
        }

        public void AddMd5Bytes()
        {
            //byte[] dataBytes = encodeBuf.ToArray();
            //byte[] computeMd5Bytes = MessageHelper.GetMd5Bytes(dataBytes);
            //encodeBuf.AddRange(computeMd5Bytes);
        }

        #endregion
    }

    public class MsgParseFailException : ApplicationException
    {
        public MsgParseFailException(int index, string message)
            : base(message)
        {
            failIndex = index;
        }

        public MsgParseFailException(string message)
            : base(message)
        {
        }

        public MsgParseFailException(string message, ApplicationException innerException)
            : base(message, innerException)
        {
        }

        private int failIndex;

        /// <summary>
        /// 解析失败的位置
        /// </summary>
        public int FailIndex
        {
            get { return failIndex; }
        }
    }
}