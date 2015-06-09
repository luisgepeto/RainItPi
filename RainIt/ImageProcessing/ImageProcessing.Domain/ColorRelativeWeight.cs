using System;

namespace ImageProcessing.Domain
{
	public class ColorRelativeWeight
	{
		public ColorRelativeWeight () : this(1,1,1)
		{		
		}
		public ColorRelativeWeight(int r, int g, int b){
			_rWeight = r;
			_gWeight = g;
			_bWeight = b;
		}
		private int _rWeight{
			get;
			set;
		}

		private int _gWeight{
			get;
			set;
		}
		private int _bWeight{
			get;
			set;
		}
		public double RWeight{
			get 
			{
				return _rWeight / TotalSum;
			}
		    set { _rWeight = (int) value; }
		}
		public double GWeight{
			get 
			{
				return _gWeight / TotalSum;
			}
            set { _gWeight = (int) value; }
		}
		public double BWeight{
			get 
			{
				return _bWeight / TotalSum;
			}
            set { _bWeight = (int) value; }
		}

		double TotalSum{
			get
			{
				var totalSum = _rWeight + _gWeight + _bWeight;
				return totalSum != 0 ? totalSum : 1;
			}
		}
	}
}

