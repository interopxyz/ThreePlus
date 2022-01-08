using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Pr = ThreePlus.Properties.Resources;

namespace ThreePlus
{
    public class CubeMap : MetaData
    {

        #region members

        protected Sd.Bitmap negX = null;
        protected Sd.Bitmap negY = null;
        protected Sd.Bitmap negZ = null;
        protected Sd.Bitmap posX = null;
        protected Sd.Bitmap posY = null;
        protected Sd.Bitmap posZ = null;

        #endregion

        #region constructors

        public CubeMap() : base()
        {
            this.negX = Properties.Resources.negx;
            this.negY = Properties.Resources.negy;
            this.negZ = Properties.Resources.negz;
            this.posX = Properties.Resources.posx;
            this.posY = Properties.Resources.posy;
            this.posZ = Properties.Resources.posz;
        }

        public CubeMap(CubeMap cubeMap) : base(cubeMap)
        {
            this.negX = new Sd.Bitmap(cubeMap.negX);
            this.negY = new Sd.Bitmap(cubeMap.negY);
            this.negZ = new Sd.Bitmap(cubeMap.negZ);
            this.posX = new Sd.Bitmap(cubeMap.posX);
            this.posY = new Sd.Bitmap(cubeMap.posY);
            this.posZ = new Sd.Bitmap(cubeMap.posZ);
        }

        public CubeMap(string name, Sd.Bitmap negX, Sd.Bitmap negY, Sd.Bitmap negZ, Sd.Bitmap posX, Sd.Bitmap posY, Sd.Bitmap posZ) : base()
        {
            this.name = name;
            this.negX = negX;
            this.negY = negY;
            this.negZ = negZ;
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
        }

        public CubeMap(Sd.Bitmap negX, Sd.Bitmap negY, Sd.Bitmap negZ, Sd.Bitmap posX, Sd.Bitmap posY, Sd.Bitmap posZ ) : base()
        {
            this.negX = negX;
            this.negY = negY;
            this.negZ = negZ;
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
        }

        public static CubeMap Earth(){return new CubeMap("Earth",Pr.EarthNegx, Pr.EarthNegy, Pr.EarthNegz, Pr.EarthPosx, Pr.EarthPosy, Pr.EarthPosz);}
        public static CubeMap Forest() { return new CubeMap("Forest",Pr.ForestNegx, Pr.ForestNegy, Pr.ForestNegz, Pr.ForestPosx, Pr.ForestPosy, Pr.ForestPosz); }
        public static CubeMap GamlaStan() { return new CubeMap("Gamla Stan", Pr.GamlaStanNegx, Pr.GamlaStanNegy, Pr.GamlaStanNegz, Pr.GamlaStanPosx, Pr.GamlaStanPosy, Pr.GamlaStanPosz); }
        public static CubeMap HeroesSquare() { return new CubeMap("Heroes Square", Pr.HeroesSquarenegx, Pr.HeroesSquarenegy, Pr.HeroesSquarenegz, Pr.HeroesSquareposx, Pr.HeroesSquareposy, Pr.HeroesSquareposz); }
        public static CubeMap Lycksele() { return new CubeMap("Lycksele", Pr.Lyckselenegx, Pr.Lyckselenegy, Pr.Lyckselenegz, Pr.Lyckseleposx, Pr.Lyckseleposy, Pr.Lyckseleposz); }
        public static CubeMap NissiBeach() { return new CubeMap("Nissi Beach", Pr.NissiBeachnegx, Pr.NissiBeachnegy, Pr.NissiBeachnegz, Pr.NissiBeachposx, Pr.NissiBeachposy, Pr.NissiBeachposz); }
        public static CubeMap Park() { return new CubeMap("Park", Pr.Parknegx, Pr.Parknegy, Pr.Parknegz, Pr.Parkposx, Pr.Parkposy, Pr.Parkposz); }
        public static CubeMap PereaBeach() { return new CubeMap("Perea Beach", Pr.PereaBeachnegx, Pr.PereaBeachnegy, Pr.PereaBeachnegz, Pr.PereaBeachposx, Pr.PereaBeachposy, Pr.PereaBeachposz); }
        public static CubeMap Pond() { return new CubeMap("Pond", Pr.Pondnegx, Pr.Pondnegy, Pr.Pondnegz, Pr.Pondposx, Pr.Pondposy, Pr.Pondposz); }
        public static CubeMap Skansen() { return new CubeMap("Skansen", Pr.Skansennegx, Pr.Skansennegy, Pr.Skansennegz, Pr.Skansenposx, Pr.Skansenposy, Pr.Skansenposz); }
        public static CubeMap SnowPark() { return new CubeMap("Snow Park", Pr.SnowParknegx, Pr.SnowParknegy, Pr.SnowParknegz, Pr.SnowParkposx, Pr.SnowParkposy, Pr.SnowParkposz); }
        public static CubeMap SwedishRoyalCastle() { return new CubeMap("Swedish Royal Castle", Pr.SwedishRoyalCastlenegx, Pr.SwedishRoyalCastlenegy, Pr.SwedishRoyalCastlenegz, Pr.SwedishRoyalCastleposx, Pr.SwedishRoyalCastleposy, Pr.SwedishRoyalCastleposz); }
        public static CubeMap Tallinn() { return new CubeMap("Tallinn", Pr.Tallinnnegx, Pr.Tallinnnegy, Pr.Tallinnnegz, Pr.Tallinnposx, Pr.Tallinnposy, Pr.Tallinnposz); }
        public static CubeMap Tantolunden() { return new CubeMap("Tantolunden", Pr.Tantolundennegx, Pr.Tantolundennegy, Pr.Tantolundennegz, Pr.Tantolundenposx, Pr.Tantolundenposy, Pr.Tantolundenposz); }
        public static CubeMap Teide() { return new CubeMap("Teide", Pr.Teidenegx, Pr.Teidenegy, Pr.Teidenegz, Pr.Teideposx, Pr.Teideposy, Pr.Teideposz); }
        public static CubeMap Vasa() { return new CubeMap("Vasa", Pr.Vasanegx, Pr.Vasanegy, Pr.Vasanegz, Pr.Vasaposx, Pr.Vasaposy, Pr.Vasaposz); }
        public static CubeMap YokohamaPark() { return new CubeMap("Yokohama Park", Pr.YokohamaParknegx, Pr.YokohamaParknegy, Pr.YokohamaParknegz, Pr.YokohamaParkposx, Pr.YokohamaParkposy, Pr.YokohamaParkposz); }
        public static CubeMap YokohamaPier() { return new CubeMap("Yokohama Pier", Pr.YokohamaPiernegx, Pr.YokohamaPiernegy, Pr.YokohamaPiernegz, Pr.posx, Pr.YokohamaPierposy, Pr.YokohamaPierposz); }

        #endregion

        #region properties

        public virtual Sd.Bitmap NegX
        {
            get { return negX; }
        }

        public virtual Sd.Bitmap NegY
        {
            get { return negY; }
        }

        public virtual Sd.Bitmap NegZ
        {
            get { return negZ; }
        }

        public virtual Sd.Bitmap PosX
        {
            get { return posX; }
        }

        public virtual Sd.Bitmap PosY
        {
            get { return posY; }
        }

        public virtual Sd.Bitmap PosZ
        {
            get { return posZ; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Cube Map | "+this.name;
        }

        #endregion
    }
}
