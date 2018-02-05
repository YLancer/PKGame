using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;


public class RulePanelScript : MonoBehaviour {
	public List<GameObject> titleImags;
	private List<string> ruleContents;
	public Text ruleContent;
	private int currentIndex = 0;

	public GameObject[] fangka;

	void Start () {

		if (GlobalDataScript.configTemp.pay == 0) {
			foreach (GameObject go in fangka) {
				go.SetActive (true);
			}
		} else {
			foreach (GameObject go in fangka) {
				go.SetActive (false);
			}
		}


		ruleContents = new List<string> ();
		ruleContents.Add ("划水麻将\n\n玩法介绍和扣分规则：\n任意一对可做将， 不能吃，只能碰，牌整了就可胡。\n1，跟庄：抓牌起的第一圈，庄家出的第一张牌，其他各玩家跟出相同的牌，庄家-3，其他各家+1. 例如庄家出5条，其他三家都打的5条。 注意此规则仅仅在第一圈有效。（牌局无论最后谁胡了或者流局都需要结算）\n2，小胡点炮 点炮者-3分\n3，小胡自摸 各家 -2，自摸者+6.\n4，大胡自摸：翻3番， 大胡包括七小对和杠上开花。 玩家 扣分 -2*3扣6分。 自摸家赢6*3=18分。 (实际相当于普通自摸*3倍)\n5，大胡捉炮：翻3番， 大胡包括七小队和抢杠胡。  玩家扣分 -3*3扣9分。  （实际相当于普通捉炮*3）。\n6，明杠各家-1， 暗杠各家-2， 玩家点杠-3（牌局无论最后谁胡了或者流局都需要结算）\n\n\n\n注意：带鱼数不参与翻番，即在原有需要扣除的分数上 加上带鱼条数*2就为实际所扣除的（乘以2是因为下鱼2条相当于各个玩家身上都有2条）\n例如， 七小对自摸下雨5条 输家各家扣除2*3+5*2=16分，  七小对捉炮： 相当于输家扣除 3*3+5*2=19分");
		ruleContents.Add ("红中麻将\n\n玩法介绍和扣分规则：\n\n无大小胡之分，任意一对可做将， 不能吃，只能碰，牌整了就可胡，有一个规则（很重要：在一圈内（这里的一圈标识一人抓了一次牌）如若A玩家听牌胡 二 五条 ，B玩家打出 二五条此时A玩家没有选择胡牌，那么在这一圈内，C D玩家如果也打得出二五条，A玩家不能胡牌，直到A玩家下一次摸牌后（自摸可以）， 此时有人打了就可以胡了，如若又没胡，那么在这一圈内 其他玩家打的也不能胡）\n扣分： 点炮-1 点杠-3，明杠各家-1，暗杠各家-2，自摸各家-2.（杠和胡牌的分数是累加的，有杠的玩家不管这把牌胡没胡分数都得算在这把的结算里）\n\n是否自摸胡（表示是否自能是自摸，玩家点炮不能胡）\n是否抢杠胡 （标识是否可抢明杠胡，别的玩家明杠，其他玩家若听牌胡此牌则可抢胡，明杠就不算了，暗杠不能抢胡）\n是否红中赖子（胡牌时红中可当任意牌使用，但不能与其他牌组合杠，如一个红中一个六筒不能碰6筒，纯红中的话可碰可杠（一般没有玩家会这么做））\n是否抓码  （可选择2 4 6个码，即胡牌的玩家最后可抓牌的数量，如果所剩的牌少于设置的抓码数量，则抓剩余的数量， 159，自己，26下家，37，对家，48上家，举例A玩家自摸其他各玩家-2，此房间设置的抓马数量是2，则最后抓两张牌，一个1和7， 1代表中自己（各家再-2），7是对家（对家再-2），即对家-6，其他玩家-4.）选择红中赖子的话设置了抓码的话只有1，5，9，红中，中码。\n（红中赖子玩法只可自摸或者抢杠胡，所以1，5，9，红中只会是自己，当选择红中赖子玩法时胡牌的时候没有红中赖子可以多抓一个码）。");
		ruleContents.Add ("长沙麻将\n\n创建房间可选信息：\n1，局数：4局（1张房卡）  8局（2张房卡）  16局（4张房卡）\n2，玩法选择：\n        1，是否区分庄闲：\n\t2，是否抓码（可设置个数）  备注：长沙麻将输赢分数波动叫大，最大抓码个数不能超过4个\n\n摸牌规则\n游戏在一开始，只有庄家可得到十四张牌，其余的人十三张。庄家从牌中选出一张最无用的牌丢出。此时，其它三家都有权力要那张丢出的牌。庄家的下家（右手边的玩者），有权力吃或碰那张牌，其它两家则只可碰或杠那张牌。 “ 碰 ” 比 “ 吃 ” 优先。\n\n庄家确定\nA 、第一局由系统随机分配庄家。\nB 、以后谁胡牌，下局谁做庄。\nC 、如果有人要了海底牌后却没人胡，则要海底牌的玩家下局当庄家。\nD 、如果四个玩家都不要海底牌，则下轮由第一个可以选择海底牌的玩家当庄家。\nE 、如果此局无海底牌（即海底牌被补张），则补海底牌的玩家下局当庄家。\nF ：如果出现通炮，则下轮由放炮玩家当庄家。\nG ：如果起手后，出现 2 个或 2 个以上的玩家胡（即天胡、四喜、六六顺、缺一色、板板胡五种牌型）则中鸟的玩家下局当庄家。\n\n小胡：\n起手胡：\n1 、大四喜：起完牌后，玩家手上已有四张一样的牌，即可胡牌。（四喜计分等同小胡自摸）\n2 、板板胡：起完牌后，玩家手上没有一张 2 、 5 、 8 （将牌），即可胡牌。（等同小胡自摸）\n3 、缺一色：起完牌后，玩家手上筒、索、万任缺一门，即可胡牌。（等同小胡自摸）\n4 、六六顺：起完牌后，玩家手上已有 2 个刻子（刻子：三个一样的牌），即可胡牌。（等同小胡自摸）\n\n平胡： 必须2 、 5 、 8 作将，其余成刻子或顺子或一句话，即可胡牌。\n\n备注：起手胡胡了玩家只需明牌一圈起手胡所需的牌 的内容。直到第二次摸牌。\n\n大胡：\n\n1 、天胡：单指庄家。庄家起牌后，即已经胡牌。\n2 、地胡：指闲家。当庄家打出第一张牌时，给闲家点炮。\n3 、碰碰胡：乱将，即表示任意数字的牌张都可以做将，可以是二五八数目牌做将，也可以是一三四六七九数目牌做将，以下乱将皆为此意。可碰，可杠，可自摸。\n4 、将将胡：玩家手上每一张牌都为 2 、 5 、 8 ，可碰。\n5 、清一色：乱将，有筒、索、万三种。任意一种胡牌规则皆可，可吃可碰，如果胡牌规则为大胡，则在原有大胡基础上再翻一倍。\n6 、海底捞月：最后一张牌为海底。海底胡牌，为大胡，需要将（将的种类根据当前牌型决定）。说明：长沙麻将中，海底牌可漫游。即轮到第一个玩家摸，他不要，则按照次序第二个玩家可要，依此类推。\n7 、海底炮：如果甲玩家要了海底，而又不能胡牌；乙玩家没有要得到海底，而又可以胡这张海底牌，即为乙玩家胡牌。同样如果丙和丁能胡则通胡。\n8 、七小对：胡牌时，手上任意七对牌。\n9、豪华七小对： 胡牌时，手中任意五对牌，以及有四张一样的牌。\n10、杠上开花：玩家有四张一样的牌，即可选择开杠掷骰子（听牌的情况下）或者补张  掷色子可以摸两张，并且不管听没听都不能再换手里的牌了，抓啥打啥，  补张的只能摸一张，可换牌。。。补张牌的计数方法为：从最后一张牌抓， 掷色子的牌，骰子开几点，即取倒数第几叠牌的连续两张（如最上一张没有则取下方一张）。一旦补张的牌能被开杠者胡则算大胡。\n11 、抢杠胡：暗杠不能抢杠胡， 只有明杠可以抢胡。玩家在明杠的时候，其他玩家可以胡被杠的此张牌，叫抢杠胡；\n12 、杠上炮：如果开杠者掷骰子补张，补张的牌开杠者若不能胡而其他玩家可以胡属于杠上炮，若胡，则属于杠上开花。\n13、全求人：吃、碰、补张以及杠后只剩一张（熟称单调）由别人打出或者自己摸到相同牌张即可胡牌。\n\n备注：  大胡可乱将胡，但杠上开花 杠上炮，抢杠胡都需要2 5 8 做将，除非本身就是大胡了。\n\n\n小胡自摸  各家-2\n小胡捉炮  点炮方-1，\n大胡自摸  各家-6\n大胡点炮  点炮方-6，\n\n备注：\n1，小胡与小胡可累加（如四喜+六六顺 2+2），大胡和大胡之前也可累加（如小七对+清一色 6+6）。\n2，若创建房间设置了区分庄闲则庄家在计算加输赢时需要 多加 或 多减 1分。庄闲分不累加，只计算1次。\n3，抓码，及胡牌的玩家可摸设置创建房间时设置的抓码数量的牌数，（159自己，2，6，下家，48上家，27对家，中了谁则谁需要多给一倍原本计算的分数，庄闲分不算入内）。例如玩家A为庄， 捉炮胡了一把（六六顺+小七对+清一色 2+6+6）， 点炮方为玩家C，是A的对家，B为A的上家，D为A的下家，  如果抓码数量是2，  扔出的骰子为 4（中上家）和7（中对家）  C所扣除的分数为（2+6+6）+（2+6+6）+1 = 29， B扣除的为 2 + 2 +1 = 5（2分起手胡的分数） ， D扣除的分数为 2 +1 =3\n\n\n\n\n");
		ruleContents.Add ("广东麻将推倒胡\n\n一、牌数：\n\t136张（带风）\n\t108张不带风（不带风）\n\n二、打牌：\n\t\n\t可以碰、杠，不能吃\n\t只能自摸，不能点炮\n\n三、玩法：\n可抢杠胡：\n\t玩家碰过之后，摸牌补杠，这张牌可被抢杠\n抢杠全包：\n\t被抢杠的玩家包其他三家的马粪分和胡牌分\n杠爆全包：\n\tA给B点杠，B杠上开花胡牌，此时A玩家包其他三家的所有分数\n可胡七对：\n\t七对牌型可以胡牌\n七对加翻：\n\t七对胡牌为基础分的2倍\n无鬼加翻：\n\t在没有鬼牌的情况下胡牌，胡牌总分数翻倍\n带风：\n\t加上东南西北中发白\n白板做鬼：\n\t白做当作鬼牌\n翻鬼：\n\t翻出一张牌，此牌的下一张为鬼牌。（翻2万，三万为鬼）\n算分：\n\t明杠3分\n\t暗杠6分\n\t自摸2分");

		ruleContents.Add ("瑞金麻将玩法介绍\n\n1、牌数：共140张，包括1-9万、1-9条、1-9筒、字牌（东、南、西、北、中、发、白）、花牌。\n2、庄家：第一局创建房间者为庄家，分为轮庄和霸王庄。\n3、宝牌：万能牌，不计分数使用，如宝牌是花牌，那其他几张花牌当万能牌使用，其它情况下不能作为万能牌使用。\n4、花牌：宝牌的代替牌，又称假宝\n5、三花飞牌：当花牌成为宝牌的时候，庄家起牌后有三个花算飞，不用出牌直接天胡；闲家摸牌后有三个花牌直接算飞，不用出牌，只需亮宝可以算胡牌。\n6、四宝飞牌：庄家起牌后有四宝算飞，不用出牌直接天胡；闲家摸牌后有四宝直接算飞，不用出牌，只需亮宝可以算胡牌。\n7、杠牌：\n   （1）明杠：手中有3张相同的牌，别人打出一张相同的牌，开杠、点杠玩家赔给杠牌玩家3分，其它玩家不需要出分。\n   （2）暗杠：手中有4张相同的牌，开杠，其余三个玩家每人赔杠牌者2分。\n   （3）碰杠：碰牌后又摸到一张相同的牌，开杠，其余三个玩家每人赔杠牌者1分。\n8、胡牌：\n   （1）天胡：10翻\n   （2）地胡：10翻\n   （3）飞牌：10翻\n   （4）四宝：10翻\n   （5）三花：10翻（只能是当“花”是宝牌的时候）\n   （6）自摸：2翻（包括烂糊、对对胡）\n   （7）平胡：1翻（包括烂糊、对对胡）\n9、庄家连庄：\n     出分方式叠加一分，比如：庄家连一庄，如果还是庄家胡牌，则其他三个玩家在原有的分数基础上每个玩家多出一分给庄家，如果闲家胡牌，则庄家多出一分给胡牌的闲家，其他未胡牌闲家不得分也不出分给胡牌闲家，连二庄则出二分，以此类推。");
		ruleContents.Add ("赣州冲关\n\n1.牌数：共136张牌，分为条、筒、万、字（东、南、西、北、中、发、白），无花牌\n2.出牌：逆时针出牌，可吃牌、碰牌、杠牌，字牌可吃。\n3.精牌\n（1）分为上精和下精，上精可在胡牌时充当万能牌使用。下精仅为算分使用，不能做万能牌。\n（2）精牌打出，导致吃、碰、杠、胡的玩家冲关，该玩家包赔上精分，其他分不影响。\n4.杠牌：\n（1）明杠每人1分\n（2）暗杠每人2分\n（3）碰杠每人1分\n5.胡牌类型\n（1）点炮（点炮者2番）\n（2）自摸(2番)\n（3）小七对(2番)\n（4）大七对（4番）\n（5）十三烂（2番）\n（6）七星十三烂（4番）\n（7）德国（2番+5）\n（8）德中德（4番+5）\n（9）精钓（2番，必须自摸）\n（10）抢杠（2番，杠牌者包赔所有胡牌分）\n（11）杠开（2番）\n（12）天胡（32分）\n（13）地胡（32分）\n可选规则：有精不可点炮胡。\n6.庄家\n（1）通庄，不分庄闲，庄家输赢分数与闲家一样\n（2）分庄闲，庄家输赢付2倍分\n以上玩法可选\n7.点炮：点炮者付2倍分\n8.弃胡：胡大不胡小，弃胡后，同一圈内可胡更大牌型，可以胡牌\n9.结算：无一炮多响，牌局结束时（包括流局），结算各家上精分、下精分、胡牌分、杠牌分、杠精分(杠精每人给10分)\n10.特殊玩法\n(1)上下翻精\n(2)埋地雷（牌局结束后翻下精）\n");
		ruleContents.Add ("欢乐斗牛\n\n玩法介绍：斗牛是传统牌类游戏，节奏紧凑且刺激。游戏由2-5人玩一副牌(54张），其中一家为庄家，其它玩家为闲家，庄家与所有闲家一一进行比较，牌型大者为胜，牌型小为败。\n抢庄玩法：\n玩法：①游戏由2-5人同时参与，使用54张扑克。游戏开始发5张牌，然后进行抢庄，抢庄后进行下注，下注完成后才能看到所有牌，玩家亮牌后与庄家一一比大小。\n②每一局所有玩家都可以抢庄，当大于2人抢庄时系统将在抢庄玩家中随机指定1名玩家当庄。如无人抢庄，则在所有玩家中随机指定1名玩家为庄。\n③庄家:抢庄成功的则为庄家，庄家与闲家一一比大小。\n④亮牌：玩家必须把手中的5张牌分成两组，第一组为3张，第二组为2张（三张牌点\n数合计10 20 30为有牛，J、Q、K、小王、大王为10点）\n轮庄玩法:\n首局由房主做庄，之后按顺时针轮流当庄，其它规则同上。房间人数：可自由参与2-5开始游戏。\n牌型选择：明牌分两种牌型，一张暗牌和两张暗牌，暗牌一种牌型，五张暗牌，根据自己的喜好，开房时自由选择。\n大小比较：\n①数字比较;大王>小王>K>Q>J>10>9>8>7>6>5>4>3>2>A\n②花色比较：黑桃>红桃>梅花>方块\n③牌型比较：顺牛>四炸>五花牛>牛牛>牛九>牛八……牛二>牛一>无牛\n④无牛牌比较：取其中最大一张牌比较大小，牌大的赢，大小相同比花色。\n⑤有牛牌型比较:比点数，点数相同，比花色。\n⑥牛牛牌型比较：取其中最大一张比较大小，牌大的赢，大小相同比花色。\n⑦五花牛牌型比较：取其中最大一张比较大小，牌大的赢，大小相同比花色。\n⑧四炸牌型比较：以四张相同牌的大小，比大小。\n⑨顺牛牌型比较：取其中最大一张比较大小，牌大的赢，大小相同比花色。\n\n所有的大小比较过程中均为庄家与闲家比较，闲家与闲家之间不比，闲家下注庄家不用。\n\n计分规则：\n胜利者获得积分=牌型的倍数X下注的分数\n失败者扣除积分=牌型的倍数X下注的分数。\n牌型：\n无牛               1倍\n牛一至牛六    1倍\n牛七至牛八    2倍\n牛九               3倍\n牛牛               4倍\n五花牛           5倍  如：五张牌10、J、J、Q、K(10算花牌）\n四炸               8倍  如：五张牌 8、8、8、8、k\n顺牛               10倍 如：五张牌（2、3、4、5、6、）(4、5、6、7、8）\n");
		ruleContents.Add ("推倒胡麻将\n\n牌数：\n  136张（带风）\n  108张（不带风）\n\n打牌：\n  可以碰、杠，不能吃\n  只能自摸，也能点炮\n\n玩法：\n\n不带风\n  牌墙中去掉东南西北中发白\n\n可抢杠胡\n  玩家碰过之后，摸牌补杠，这张牌可以被抢杠胡，被抢杠玩家包其他玩家分数\n\n明杠可抢\n  玩家手中3张牌，其他玩家打出第4张，此玩家直杠的时候，其他玩家可以抢杠胡\n\n抢杠全包\n 被抢杠的玩家包其他玩家的马分跟胡牌分\n\n杠爆全包\n  A给B点杠，B杠上开花胡牌，此时A玩家包其他三家的所有分数\n\n白板做鬼\n  白板当做鬼牌\n\n翻鬼\n  翻出一张牌，此张牌的下一张当做鬼牌。（翻二万，则三万是鬼）\n\n马跟底分\n  胡牌的基础分数为几分，每个马就为几分\n\n马跟杠\n  勾选马跟杠，默认荒庄荒杠，马牌会对胡牌分进行二次计算\n算分：\n  明杠 3分\n  暗杠 6分\n  基础分 2分\n\n胡牌加成\n  自摸 2分\n  点炮 3分\n\n牌型加成\n  七小对2倍\n  抢杠胡2倍\n  碰碰胡2倍\n  清一色2倍\n  杠上开花2倍\n  豪华七对4倍\n  十三幺10倍\n  天地胡10倍\n  双豪华6倍\n  全风8倍\n  花幺九6倍");
		ruleContents.Add ("逼胡麻将\n\n玩法介绍和扣分规则：\n\n无大小胡之分，任意一对可做将， 不能吃，只能碰，牌整了就可胡，有一个规则（很重要：在一圈内（这里的一圈标识一人抓了一次牌）如若A玩家听牌胡 二 五条 ，B玩家打出 二五条此时A玩家没有选择胡牌，那么在这一圈内，C D玩家如果也打得出二五条，A玩家不能胡牌，直到A玩家下一次摸牌后（自摸可以）， 此时有人打了就可以胡了，如若又没胡，那么在这一圈内 其他玩家打的也不能胡）\n扣分： 点炮-1 点杠-3，明杠各家-1，暗杠各家-2，自摸各家-2.（杠和胡牌的分数是累加的，有杠的玩家不管这把牌胡没胡分数都得算在这把的结算里）\n\n是否自摸胡（表示是否自能是自摸，玩家点炮不能胡）\n是否抢杠胡 （标识是否可抢明杠胡，别的玩家明杠，其他玩家若听牌胡此牌则可抢胡，明杠就不算了，暗杠不能抢胡）\n是否红中赖子（胡牌时红中可当任意牌使用，但不能与其他牌组合杠，如一个红中一个六筒不能碰6筒，纯红中的话可碰可杠（一般没有玩家会这么做））\n是否抓码  （可选择2 4 6个码，即胡牌的玩家最后可抓牌的数量，如果所剩的牌少于设置的抓码数量，则抓剩余的数量， 159，自己，26下家，37，对家，48上家，举例A玩家自摸其他各玩家-2，此房间设置的抓马数量是2，则最后抓两张牌，一个1和7， 1代表中自己（各家再-2），7是对家（对家再-2），即对家-6，其他玩家-4.）选择红中赖子的话设置了抓码的话只有1，5，9，红中，中码。\n（红中赖子玩法只可自摸或者抢杠胡，所以1，5，9，红中只会是自己，当选择红中赖子玩法时胡牌的时候没有红中赖子可以多抓一个码）。");
		ruleContents.Add ("跑得快\n\n跑得快玩法规则：\n牌库\n一副牌，去掉大小王、三个2（保留黑桃2）和黑桃A，共48张，每人16张。（也就是说，牌库中只有一张2,三张A，无大小王）\n\n参与人数\n3人\n\n游戏过程\n摸到红桃3的玩家先出牌，最先出完手中16张牌的人获胜。\n\n出牌规则\n1.逆时针出牌；\n2.上家出的牌，下家有出必出，放走包赔。（放走规则：当下家报单，如果选择出单张，必须为手牌中的最大牌，否则视为放走，需包赔所有输分）；\n3.炸弹赢取当圈的玩家即刻可获得由另2位玩家给予的喜金。\n\n可出牌型\n单张：1张任意牌，2最大，3最小； \n\n对子：2张点数相同的牌，AA最大，33最小； \n\n3带X：3同张最多可带2张（0≤X≤2），带的牌不要求同点数；接牌时，必须按上家的3+N接；\n\n顺子：点数相连的5张及以上的牌，可以从3连到A。如：910JQKA；\n\n连对：点数相连的2张及以上的对子，可以从3连到A。如：7788，334455；\n\n三顺：点数相连的2个及以上的3同张，可以从3连到A。如：888999；\n\n飞机带翅膀：点数相连的2个及以上的3同张，可以从3连到A。连N个3同张，则可带N张牌或者2N张牌。如：连3个3同张，333444555，可选择带3张牌，也可以选择带6张牌。连2个3同张，333444，可选择带2张牌，也可带4张牌。\n\n炸弹：4张相同点数的牌。如： 6666；炸弹的大小和牌点大小规则相同。\n\n4带X：当玩家选择四张带牌的时候，四张不算炸弹，无法获得炸弹规则中的额外奖励。四带N可最多可带3张牌（0≤X≤3）。注：4带1=3带2");
		ruleContents.Add ("德州扑克\n\n玩法说明：\n牌组大小规则：同花顺＞四条＞葫芦＞同花＞顺子＞三条＞两对＞一对＞单牌。\n德州扑克一共押四轮注：\n每人发2张牌后进行第一轮押注；\n发3张公共牌后押第二轮注；\n发第4张公共牌后押第三轮注；\n发第5张公共牌后押第四轮注。押注结束后所有剩余玩家进行比牌，最大者赢得底池。\n\n皇家同花顺\n同花色的A, K, Q, J和10.\n平手牌：在摊牌的时候有两副多副皇家同花顺时，平分筹码。\n同花顺\n五张同花色的连续牌。\n 平手牌：如果摊牌时有两副或多副同花顺，连续牌的头张牌大的获得筹码。如果是两副或多副相同的连续牌，平分筹码。\n四条\n其中四张是相同点数但不同花的扑克牌，第五张是随意的一张牌\n平手牌：如果两组或者更多组摊牌，则四张牌中的最大者赢局，如果一组人持有\n的四张牌是一样的，那么第五张牌最大者赢局（起脚牌）。如果起脚牌也一样，平分彩池。\n满堂彩（葫芦，三带二）\n由三张相同点数及任何两张其他相同点数的扑克牌组成\n平手牌：如果两组或者更多组摊牌，那么三张相同点数中较大者赢局。如果三张牌都一样，则两张牌中\n点数较大者赢局，如果所有的牌都一样，则平分彩池。\n同花\n此牌由五张不按顺序但相同花的扑克牌组成\n平手牌：如果不止一人抓到此牌相，则牌点最高的人赢得该局，如果最大点相同，则由第二、第三、第四或者第五张牌来决定胜负，如果所有的牌都相同，平分彩池。\n顺子\n此牌由五张顺序扑克牌组成\n平手牌：如果不止一人抓到此牌，则五张牌中点数最大的赢得此局，如果所有牌点数都\n相同，平分彩池。\n三条\n由三张相同点数和两张不同点数的扑克组成\n平手牌：如果不止一人抓到此牌，则三张牌中最大点数者赢局，如果三张牌都相同，比较第四\n张牌，必要时比较第五张，点数大的人赢局。如果所有牌都相同，则平分彩池。\n两对\n两对点数相同但两两不同的扑克和随意的一张牌组成\n平手牌：如果不止一人抓大此牌相，牌点比较大的人赢，如果比较大的牌点相同，那么较小\n牌点中的较大者赢，如果两对牌点相同，那么第五张牌点较大者赢（起脚牌）。如果起脚牌也相同，\n则平分彩池。\n一对\n由两张相同点数的扑克牌和另三张随意的牌组成\n平手牌：如果不止一人抓到此牌，则两张牌中点数大的赢，如果对牌都一样，则比较另外三张牌\n中大的赢，如果另外三张牌中较大的也一样则比较第二大的和第三大的，如果所有的牌都一样，\n则平分彩池。\n单张大牌\n既不是同一花色也不是同一点数的五张牌组成。\n平手牌：如果不止一人抓到此牌，则比较点数最大者，如果点数最大的相同，则比较第二、第三、第四和第五大的，如果所有牌都相同，则平分彩池。\n");

		chaoshan_click ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void closeDialog(){
		RulePanelScript self = GetComponent<RulePanelScript> ();
		for (int i = 0; i < titleImags.Count; i++) {
			GameObject item = titleImags [i];
			Destroy (item);
		}
		Destroy (self.ruleContent);

		self = null;
		Destroy (this);
		Destroy (gameObject);
	}

	public void ruleDown(){
        SoundCtrl.getInstance().playSoundByActionButton(1);
		if (currentIndex > 0) {
			titleImags [currentIndex - 1].SetActive (true);
			titleImags [currentIndex].SetActive (false);
			ruleContent.text = ruleContents [currentIndex - 1];
            ruleContent.transform.position =  new Vector3( ruleContent.transform.position.x, -ruleContent.preferredHeight/2 + 220 - 100, 0); //调整文本框y值，省得看不见
            currentIndex -= 1;
		}
	}
	public void ruleUp(){
        SoundCtrl.getInstance().playSoundByActionButton(1);
        if (currentIndex <titleImags.Count-1) {
			titleImags [currentIndex +1].SetActive (true);
			titleImags [currentIndex].SetActive (false);
			ruleContent.text = ruleContents [currentIndex + 1];
            ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -ruleContent.preferredHeight / 2 + 220 - 100, 0);//调整文本框y值，省得看不见
            currentIndex += 1;

		}
	}

	


    public GameObject ruijin;
    public GameObject ruijin1;
    public GameObject ganzhou;
	public GameObject ganzhou1;
    public GameObject dn;
    public GameObject dn1;
	public GameObject guangdong;
	public GameObject guangdong1;
	public GameObject chaoshan;
	public GameObject chaoshan1;
	public GameObject bihu;
	public GameObject bihu1;
	public GameObject pdk;
	public GameObject pdk1;
	public GameObject dzpk;
	public GameObject dzpk1;

    public void ruijin_click()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(true);
		ruijin1.SetActive(false);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

        ruleContent.text = ruleContents[4];
        ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
    }

    public void ganzhou_click(){
		/**
		TipsManagerScript.getInstance ().setTips ("开发中");
		return;
		*/

		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(true);
		ganzhou1.SetActive(false);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

        ruleContent.text = ruleContents[5];
        ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
    }

    public void dn_click()
    {
        SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(true);
		dn1.SetActive(false);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

        ruleContent.text = ruleContents[6];
        ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
    }

	public void guangdong_click()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (true);
		guangdong1.SetActive (false);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

		ruleContent.text = ruleContents[3];
		ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
	}

	public void chaoshan_click()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (true);
		chaoshan1.SetActive (false);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

		ruleContent.text = ruleContents[7];
		ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
	}

	public void bihu_click()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (true);
		bihu1.SetActive (false);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

		ruleContent.text = ruleContents[8];
		ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
	}

	public void pdk_click()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (true);
		pdk1.SetActive (false);
		dzpk.SetActive (false);
		dzpk1.SetActive (true);

		ruleContent.text = ruleContents[9];
		ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
	}

	public void dzpk_click()
	{
		SoundCtrl.getInstance().playSoundByActionButton(1);

		ruijin.SetActive(false);
		ruijin1.SetActive(true);
		ganzhou.SetActive(false);
		ganzhou1.SetActive(true);
		dn.SetActive(false);
		dn1.SetActive(true);
		guangdong.SetActive (false);
		guangdong1.SetActive (true);
		chaoshan.SetActive (false);
		chaoshan1.SetActive (true);
		bihu.SetActive (false);
		bihu1.SetActive (true);
		pdk.SetActive (false);
		pdk1.SetActive (true);
		dzpk.SetActive (true);
		dzpk1.SetActive (false);

		ruleContent.text = ruleContents[10];
		ruleContent.transform.position = new Vector3(ruleContent.transform.position.x, -1.5f, 0);//调整文本框y值，省得看不见
	}

}
