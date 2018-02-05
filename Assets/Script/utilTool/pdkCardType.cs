using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class pdkCardType
	{
		public enum CARDTYPE {
			c0, // 不能出牌
			c1, // 单牌。
			c2, // 对子。
			c3, // 3不带。
			c4, // 炸弹。
			c31, // 3带1。
			c311,//3带2
			c32, // 3带对。
			c411, // 4带2个单，或者一对
			c422, // 4带2对
			c12345, // 连子。
			c1122, // 连队。
			c112233, // 三连及以上
			c111222, // 飞机。
			c11122234, // 飞机带单排
			c111222N,//飞机随机带
			c1112223456, // 飞机带4子
			c1112223344// 飞机带对子.
		}

		public pdkCardType ()
		{
			
		}

		public bool compareType(int[] oldCardArray, int[] newCardArray) {
			if (oldCardArray == null || oldCardArray.Length == 0)
				return false;
			if (newCardArray == null || newCardArray.Length == 0)
				return false;
			
			CARDTYPE type1 = getType(oldCardArray);
			CARDTYPE type2 = getType(newCardArray);

			if(type1 == type2) {
				int[] oldCard = new int[oldCardArray.Length];
				for (int i = 0; i < oldCardArray.Length; i++) {
					oldCard[i] = oldCardArray[i] % 13;
				}
				// 从小到大排序
				Array.Sort(oldCard);

				int[] newCard = new int[newCardArray.Length];
				for (int i = 0; i < newCardArray.Length; i++) {
					newCard[i] = newCardArray[i] % 13;
				}
				// 从小到大排序
				Array.Sort(newCard);

				if(type1 == CARDTYPE.c1) {//单牌
					if(oldCard[0]<newCard[0])
						return true;
				}else if(type1 == CARDTYPE.c2) {// 对子
					if(oldCard[0]<newCard[0])
						return true;
				}else if(type1 == CARDTYPE.c112233) {//连对
					if(newCard.Length == oldCard.Length
						&& oldCard[0]<newCard[0])
						return true;
				}else if(type1 == CARDTYPE.c311) {//三带2
					List<int>[] a = getCount(oldCard);
					List<int>[] b = getCount(newCard);
					if( (int)a[2][0] < (int)b[2][0]) {
						return true;
					}
				}else if(type1 == CARDTYPE.c1112223456) {//飞机带翅膀
					List<int>[] a = getCount(oldCard);
					List<int>[] b = getCount(newCard);
					if(oldCard.Length == newCard.Length && (int)a[2][0] < (int)b[2][0])
						return true;
				}else if(type1 == CARDTYPE.c12345) {//顺子
					if(oldCard.Length == newCard.Length && oldCard[0] < newCard[0])
						return true;
				}else if(type1 == CARDTYPE.c4) {//炸弹
					if(oldCard[0] < newCard[0])
						return true;
				}

			}else {
				if(type2 == CARDTYPE.c4) {//后面的是炸弹
					return true;
				}
			}

			return false;
		}

		public bool isCanChi(int[] chuCard_old, int[] myCardArray_old) {
			CARDTYPE type1 = getType(chuCard_old);

			int[] chuCard = new int[chuCard_old.Length];
			for (int i = 0; i < chuCard_old.Length; i++) {
				chuCard[i] = chuCard_old[i] % 13;
			}
			// 从小到大排序
			Array.Sort(chuCard);


			int count=0;
			for (int i = 0; i < myCardArray_old.Length; i++) {
				if(myCardArray_old[i]>0) {
					count++;
				}
			}
			int[] myCardArray = new int[count];
			count = 0;
			for (int i = 0; i < myCardArray_old.Length; i++) {
				if(myCardArray_old[i]>0) {
					myCardArray[count] = i % 13;
					count++;
				}
			}

			// 从小到大排序
			Array.Sort(myCardArray);

			List<int>[] a=getCount(chuCard);
			List<int>[] b=getCount(myCardArray);

			int length = myCardArray.Length;

			if(type1 == CARDTYPE.c1) {//单牌
				if(b[3].Count>0)
					return true;
				if(chuCard[0]<myCardArray[length-1])
					return true;
			}else if(type1 == CARDTYPE.c2) {// 对子
				if(b[3].Count>0)
					return true;
				if(b[2].Count >0 && chuCard[0]< (int)b[2][b[2].Count-1] )
					return true;
				if(b[1].Count >0 && chuCard[0]< (int)b[1][b[1].Count-1] )
					return true;
			}else if(type1 == CARDTYPE.c112233) {//连对
				if(b[3].Count>0)
					return true;

				//首先得手牌超过上家出的张数 
				if(length >= chuCard.Length) {
					//循环遍历玩家手牌 是否有大于出牌的
					for(int i= chuCard[0]+1; i<13; i++) {
						int findCount = 0;
						for(int j=0;j<chuCard.Length/2;j++) {
							//遍历2张的。
							for(int k=0;k<b[1].Count;k++) {
								if((int)b[1][k] !=12 && (int)b[1][k]  == i+j) {
									findCount++;
									break;
								}
							}
							//遍历3张的。
							for(int k=0;k<b[2].Count;k++) {
								if((int)b[2][k] !=12 && (int)b[2][k]  == i+j) {
									findCount++;
									break;
								}
							}
						}

						if(findCount >=chuCard.Length/2)
							return true;

					}
				}
			}else if(type1 == CARDTYPE.c311) {//三带2
				if(b[3].Count>0)
					return true;

				//首先得手牌超过上家出的张数
				if(length >=chuCard.Length && b[2].Count>0 && (int)a[2][0] < (int)b[2][b[2].Count-1]) {
					return true;
				}
			}else if(type1 == CARDTYPE.c1112223456) {//飞机带翅膀
				if(b[3].Count>0)
					return true;

				//首先得手牌超过上家出的张数 
				if(length >= chuCard.Length) {
					//循环遍历玩家手牌 是否有大于出牌的
					for(int i= (int)a[2][0]+1; i<13; i++) {
						int findCount = 0;
						for(int j=0;j<chuCard.Length/5;j++) {
							//遍历3张的。
							for(int k=0;k<b[2].Count;k++) {
								if((int)b[2][k] !=12 && (int)b[2][k]  == i+j) {
									findCount++;
									break;
								}
							}
							//遍历4张的。
							for(int k=0;k<b[3].Count;k++) {
								if((int)b[3][k] !=12 && (int)b[3][k]  == i+j) {
									findCount++;
									break;
								}
							}
						}

						if(findCount >=chuCard.Length/5)
							return true;

					}
				}
			}else if(type1 == CARDTYPE.c12345) {//顺子
				if(b[3].Count>0)
					return true;
				//首先得手牌超过上家出的张数 
				if(length >= chuCard.Length) {
					//循环遍历玩家手牌 是否有大于出牌的
					for(int i= chuCard[0]+1; i<13; i++) {
						int findCount = 0;
						for(int j=0;j<chuCard.Length;j++) {
							for(int k=0;k<length;k++) {
								if(myCardArray[k] !=12 && myCardArray[k] == i+j) {
									findCount++;
									break;
								}
							}
						}

						if(findCount >=chuCard.Length)
							return true;

					}
				}
			}else if(type1 == CARDTYPE.c4) {//炸弹
				if(b[3].Count>0 && chuCard[0]< (int)b[3][b[3].Count-1])
					return true;
			}

			return false;
		}

		// 跑得快牌型
		public CARDTYPE getType(int[] incard) {

			int[] card = new int[incard.Length];
			for (int i = 0; i < incard.Length; i++) {
				card[i] = incard[i] % 13;
			}

			// 从小到大排序
			Array.Sort(card);

			int length = card.Length;

			if (card.Length == 0) {
				return CARDTYPE.c0;
			} else if (card.Length == 1) {
				return CARDTYPE.c1;
			} else if (card.Length == 2) {
				if (card[0] == card[1]) {
					return CARDTYPE.c2;
				}
			} else if (card.Length == 3) {
				if (card[0] == card[1] && card[1] == card[2]) {
					return CARDTYPE.c3;
				}
			} else if (card.Length == 4) {
				if (card[0] == card[1] && card[1] == card[2] && card[2] == card[3]) {
					return CARDTYPE.c4;
				}

				if (card[0] == card[2] || card[1] == card[3]) {
					return CARDTYPE.c31;
				}

				if (card[0] == card[1] && card[2] == card[3] && card[2] - card[1] == 1) {
					return CARDTYPE.c112233;// 11 22 二连对
				}

			} else if (card.Length >= 5) {
				// 我们先定义一个List a[4]，其中a[0]的值为list中重复一次(单张牌)的牌的个数，
				// a[1]的值为list中重复二次(对牌)的牌的个数，a[2]的值为list中重复三次(三张)的牌的个数，
				// a[3]的值为list中重复四次(炸弹)的牌的个数。
				List<int>[] a = getCount(card);

				if (card.Length == 5 && a[2].Count == 1) {
					return CARDTYPE.c311;// 444 99
				}

				if (card.Length >= 5 && a[0].Count == card.Length && (card[length - 1] - card[0] == length - 1)
					&& card[length - 1] < 12) {// 顺子不能带2 最多到A
					return CARDTYPE.c12345;// 12345 或 1234567等
				}

				if (card.Length >= 6 && length % 2 == 0 && a[1].Count == length / 2
					&& (card[length - 1] - card[0] == length / 2 - 1) && card[length - 1] < 12) {// 顺子不能带2 最多到A
					return CARDTYPE.c112233;// 12345 或 1234567等
				}

				if (a[2].Count >= 2 && a[2].Count == length / 5 && length % 5 == 0
					&& ((int) a[2][a[2].Count - 1] - (int) a[2][0] == length / 5 - 1)
					&& (int) a[2][a[2].Count - 1]<12) {//最多到A
					return CARDTYPE.c1112223456; // 111222 3456
				}

				if (a[2].Count >= 2
					&& ((int) a[2][a[2].Count - 1] - (int) a[2][0] == a[2].Count - 1)
					&& (int) a[2][a[2].Count - 1]<12) {//最多到A
					return CARDTYPE.c111222N; // 111222 34
				}
			}

			return CARDTYPE.c0;
		}


		public List<int>[] getCount(int[] card) {
			// 我们先定义一个List a[4]，其中a[0]的值为list中重复一次(单张牌)的牌的个数，
			// a[1]的值为list中重复二次(对牌)的牌的个数，a[2]的值为list中重复三次(三张)的牌的个数，
			// a[3]的值为list中重复四次(炸弹)的牌的个数。
			List<int>[] a = new List<int>[4];
			a[0] = new List<int>();
			a[1] = new List<int>();
			a[2] = new List<int>();
			a[3] = new List<int>();

			for (int i = 0; i < card.Length; i++) {
				int count = 1;
				for (int j = i + 1; j < card.Length; j++) {
					if (card[i] == card[j]) {
						count++;
						i = j;// 跳过重复的数字
					} else {
						break;
					}
				}

				if (count == 4) {
					a[3].Add(card[i]);
				} else if (count == 3) {
					a[2].Add(card[i]);
				} else if (count == 2) {
					a[1].Add(card[i]);
				} else if (count == 1) {
					a[0].Add(card[i]);
				}
			}

			return a;
		}
	}
}

