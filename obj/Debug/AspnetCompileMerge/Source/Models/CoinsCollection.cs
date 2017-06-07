//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace collectCoins.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CoinsCollection
    {
        public int CoinId { get; set; }
        public int GeneralCoinId { get; set; }
        public int PortretDesignerId { get; set; }
        public int ReverseDesignerId { get; set; }
        public Nullable<int> EdgeId { get; set; }
        public int Year { get; set; }
        public Nullable<long> Mintage { get; set; }
        public string EstimatedValues { get; set; }
        public string Image { get; set; }
    
        public virtual MyCollection MyCollection { get; set; }
        public virtual EdgeInscription EdgeInscription { get; set; }
        public virtual GeneralCoin GeneralCoin { get; set; }
        public virtual PortretDesigner PortretDesigner { get; set; }
        public virtual ReverseDesigner ReverseDesigner { get; set; }
    }
}