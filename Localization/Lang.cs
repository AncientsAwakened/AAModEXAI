using Terraria.Localization;

namespace AAModEXAI
{
    public class Trans
    {
        // 0 is English
        // 1 is Chinese
        // 2 is Russian
        public static string text(string place, string info)
        {
            int index = 0;
            if(Language.ActiveCulture == GameCulture.Chinese)
            {
                index = 1;
            }
            else if(Language.ActiveCulture == GameCulture.Russian)
            {
                index = 2;
            }

            string Info = info;

            Translation(place, ref info, index);
            if(info == Info) Translation(place, ref info, 0);

            return info;
        }

        public static void Translation(string place, ref string info, int index)
        {
            if(place == "Common") Common(ref info, index);
            else if(place == "AH") AHChat(ref info, index);
            else if(place == "Akuma") AkumaChat(ref info, index);
            else if(place == "Anubis") AnubisChat(ref info, index);
            else if(place == "Rajah") RajahChat(ref info, index);
            return;
        }

        public static void Common(ref string info, int index)
        {
            if(info == "male")
            {
                if(index == 0) info = "him";
                else if(index == 1) info = "他";
                else if(index == 2) info = "его";
            }
            if(info == "famale")
            {
                if(index == 0) info = "her";
                else if(index == 1) info = "她";
                else if(index == 2) info = "ее";
            }
            if(info == "malebig")
            {
                if(index == 0) info = "HIM";
                else if(index == 1) info = "他";
                else if(index == 2) info = "ЕГО";
            }
            if(info == "famalebig")
            {
                if(index == 0) info = "HER";
                else if(index == 1) info = "她";
                else if(index == 2) info = "ЕЕ";
            }
            if(info == "boy")
            {
                if(index == 0) info = "BOY";
                else if(index == 1) info = "少年";
                else if(index == 2) info = "МАЛЬЧИК";
            }
            if(info == "girl")
            {
                if(index == 0) info = "GIRL";
                else if(index == 1) info = "少女";
                else if(index == 2) info = "ДЕВОЧКА";
            }

            if(info == "NPCarrive")
            {
                if(index == 0) info = " has awoken!";
                else if(index == 1) info = " 到了！";
                else if(index == 2) info = " прибыл!";
            }
            if(info == "BosshasAwoken")
            {
                if(index == 0) info = " has awoken!";
                else if(index == 1) info = " 已苏醒！";
                else if(index == 2) info = " пробудился!";
            }
            if(info == "BosshaveAwoken")
            {
                if(index == 0) info = " have awoken!";
                else if(index == 1) info = " 已苏醒！";
                else if(index == 2) info = " пробудились!";
            }

            if(info == "RajahStatueInfo")
            {
                if(index == 0) info = "When Bodhisattva Avalokiteshvara was practicing the profound Prajna Paramita, he illuminated the Five Skandhas and saw that they are all empty, and he crossed beyond all suffering and difficulty.";
                else if(index == 1) info = "观自在菩萨，行深般若波罗蜜多时，照见五蕴皆空，度一切苦厄";
            }
        }

        public static void AHChat(ref string info, int index)
        {
            if(info == "AHDeath1")
            {
                if(index == 0) info = "RRRRRRRRRGH! NOT AGAIN!!!";
                else if(index == 1) info = "啊啊啊啊啊! 别又这样了!!!";
                else if(index == 2) info = "РРРРРРРГХ! НЕТ, ТОЛЬКО НЕ СНОВА!!!";
            }
            if(info == "AHDeath2")
            {
                if(index == 0) info = "You see Ashe? I told you this was a stupid idea, but YOU didn't listen...";
                else if(index == 1) info = "艾希你看? 我告诉你这主意很蠢, 但是你就是不听...";
                else if(index == 2) info = "Видишь, Аши?? Я говорила тебе, что это тупая идея, но ТЫ не слушаешь...";
            }
            if(info == "AHDeath3")
            {
                if(index == 0) info = "Why do I keep going with you..? I should honestly just let you fight them yourself.";
                else if(index == 1) info = "我为什么要跟你一块. . ? 我真应该让你自己和他们打. ";
                else if(index == 2) info = "Почему я снова и снова иду с тобой..? Если честно, то я уже просто должна оставить тебя один на один с ними.";
            }
            if(info == "AHDeath4")
            {
                if(index == 0) info = "Shut up! I thought if we ganged up on ";
                else if(index == 1) info = "闭嘴吧! 我觉得我们合力攻击";
                else if(index == 2) info = "Заткнись! Я думала, что если мы объединимся ";
            }
            if(info == "AHDeath5")
            {
                if(index == 0) info = ", we could just beat the daylights out of 'em!";
                else if(index == 1) info = ", 我们应该可以把他打趴下!";
                else if(index == 2) info = ", то сможем уничтожить их!";
            }
            if(info == "AHDeath6")
            {
                if(index == 0) info = "Rgh..! Shut up..!";
                else if(index == 1) info = "啊. . ! 闭嘴. . !";
                else if(index == 2) info = "Рхг..! Заткнись..!";
            }
            if(info == "AHDeath7")
            {
                if(index == 0) info = "Whatever...I'm going back home. SOMEONE has to tell dad about this kid.";
                else if(index == 1) info = "无论怎样...我要回去了. 必须有人告诉父亲这臭小子的事.";
                else if(index == 2) info = "В любом случае... Я иду домой. КТО-ТО должен расказать папе об этом.";
            }
            if(info == "AHDeath8")
            {
                if(index == 0) info = "Hmpf..! Fine! Be that way! I'm going back to the inferno!";
                else if(index == 1) info = "好好好. . ! 好吧! 从这边走! 我要回燎狱!";
                else if(index == 2) info = "Хмпф..! Ладно! Я возвращаюсь в Инферно!";
            }

            if(info == "AHSpawn1")
            {
                if(index == 0) info = "Well hello there, what a surprise to see YOU here~!";
                else if(index == 1) info = "嗯嗯, 你好, 在这看见你真惊喜~!.";
                else if(index == 2) info = "Ну привет, какой сюрприз увидеть ТЕБЯ здесь~!";
            }
            if(info == "AHSpawn2")
            {
                if(index == 0) info = "Oh yes, I've heard PLENTY about you, kid...you've been stirring up quite a bit of trouble in these parts...";
                else if(index == 1) info = "啊对, 我听说过你的 相 当 多 的 好 事, 臭小子...你在这些地方可搞了不少的麻烦...";
                else if(index == 2) info = "Ах, да, я про тебя много слышала, сопляк... ты создал нам кучу проблем...";
            }
            if(info == "AHSpawn3")
            {
                if(index == 0) info = "You're a pretty annoying wretch, you know...";
                else if(index == 1) info = "你应该知道, 你就是个非常讨厌的家伙...";
                else if(index == 2) info = "Ты очень надоедлив...";
            }
            if(info == "AHSpawn4")
            {
                if(index == 0) info = "So now..! Heh...";
                else if(index == 1) info = "所以现在. . !";
                else if(index == 2) info = "Так что теперь..! Хехе...";
            }
            if(info == "AHSpawn5")
            {
                if(index == 0) info = "We're gonna give you something to absolutely SCREAM about..! Come on, Hakie, let's torch this little warm-blood~!";
                else if(index == 1) info = "我们决定打的你 嗷 嗷 叫. . ! 来吧, 遥酱, 让我们来教训教训这个常温东西~!";
                else if(index == 2) info = "Мы заставим тебя кричать от боли! Давай, Хаки, сожжем этого теплокровного~!";
            }
            if(info == "AHSpawn6")
            {
                if(index == 0) info = "Please don't call me Hakie again...ever.";
                else if(index == 1) info = "别再叫我遥酱了...从现在起.";
                else if(index == 2) info = "Пожалуйста, больше никогда не называй меня Хаки...";
            }

            if(info == "HarukaDowned")
            {
                if(index == 0) info = "Rgh..! Ow...";
                else if(index == 1) info = "啊. . ! 哦啊...";
                else if(index == 2) info = "Ргх..! Ау...";
            }
            if(info == "AsheDowned")
            {
                if(index == 0) info = "OW..! THAT HURT, YOU MEANIE!";
                else if(index == 1) info = "哦 啊. . ! 很 疼, 你 这 个 小 鬼!";
                else if(index == 2) info = "АУЧ..! ВООБЩЕ ТО БОЛЬНО, ПРИДУРОК!";
            }
        }

        public static void AkumaChat(ref string info, int index)
        {
            if(info == "Akuma1")
            {
                if(index == 0) info = "Water?! ACK..! I CAN'T BREATHE!";
                else if(index == 1) info = "水? ! 卧槽. . ! 我没法呼吸了!";
                else if(index == 2) info = "Вода?! АК..! Я НЕ МОГУ ДЫШАТЬ!";
            }
            if(info == "Akuma2")
            {
                if(index == 0) info = "Yaaaaaaaaawn. I'm bushed kid, I'm gonna have to take a rain check. Come back tomorrow.";
                else if(index == 1) info = "呀呀呀呀呀呀. 我累了, 小子, 咱俩的比试要推迟了. 明天再来吧. ";
                else if(index == 2) info = "*Зевает* Прости, сопляк, я устал. Мне надо выспаться. Возвращайся завтра.";
            }
            if(info == "Akuma3")
            {
                if(index == 0) info = "I thought you terrarians put up more of a fight. Guess not.";
                else if(index == 1) info = "我以为你们这些泰拉人更喜欢打架. 看来猜错了. ";
                else if(index == 2) info = "А я думал что вы, террарианы, даете более серьезный бой. Похоже нет.";
            }
            if(info == "Akuma4")
            {
                if(index == 0) info = "Hey kid! Sky's fallin', watch out!";
                else if(index == 1) info = "呀呀呀呀呀呀. 我累了, 小子, 咱俩的比试要推迟了. 明天再来吧. ";
                else if(index == 2) info = "*Зевает* Прости, сопляк, я устал. Мне надо выспаться. Возвращайся завтра.";
            }
            if(info == "Akuma5")
            {
                if(index == 0) info = "Down comes fire and fury!";
                else if(index == 1) info = "烈火与愤怒从天而降!";
                else if(index == 2) info = "Вниз спускаются огонь и ярость!";
            }
            if(info == "Akuma6")
            {
                if(index == 0) info = "Spirits of the volcano! help me crush this kid!";
                else if(index == 1) info = "燎狱火山之灵啊!助我压扁这小子!";
                else if(index == 2) info = "Духи вулкана! Помогите мне раздавить этого сопляка!";
            }
            if(info == "Akuma7")
            {
                if(index == 0) info = "Hey kid! Watch out!";
                else if(index == 1) info = "嘿小子!小心!";
                else if(index == 2) info = "Эй, сопляк! Смотри!";
            }
            if(info == "Akuma8")
            {
                if(index == 0) info = "Incoming!";
                else if(index == 1) info = "我来了!";
                else if(index == 2) info = "Смотри!";
            }
            if(info == "Akuma9")
            {
                if(index == 0) info = "Sun's shining, and there's no shade to be seen, kid!";
                else if(index == 1) info = "小子, 阳光普照之处没有阴凉可乘!";
                else if(index == 2) info = "Солнце светит, и тени нигде не видно, сопляк!";
            }
            if(info == "Akuma10")
            {
                if(index == 0) info = "Getting hotter, ain't it?";
                else if(index == 1) info = "热起来了, 对吗?";
                else if(index == 2) info = "Становится жарковато, не так ли?";
            }
            if(info == "Akuma11")
            {
                if(index == 0) info = "The volcanoes of the inferno are finally quelled...";
                else if(index == 1) info = "燎狱的火山终于平息了...";
                else if(index == 2) info = "Вулканы Инферно наконец успокоены...";
            }
            if(info == "Akuma12")
            {
                if(index == 0) info = "Hmpf...you’re pretty good kid, but not good enough. Come back once you’ve gotten a bit better.";
                else if(index == 1) info = "哈...你小子还不错, 但还不够强. 等你再强一点再回来找我. ";
                else if(index == 2) info = "Хмпф... ты хорош, сопляк, но не достаточно хорош. Возвращайся, когда будешь получше.";
            }
            if(info == "Akuma13")
            {
                if(index == 0) info = "Face the flames of despair, kid!";
                else if(index == 1) info = "面对绝望之火吧!小子!";
                else if(index == 2) info = "Встань лицом к лицу с огнями безнадеги, сопляк!";
            }
            if(info == "Akuma14")
            {
                if(index == 0) info = "Heads up, kid!";
                else if(index == 1) info = "注意力集中, 小子!";
                else if(index == 2) info = "Спасайся, сопляк!";
            }

            if(info == "AkumaTransition1")
            {
                if(index == 0) info = "Heh...";
                else if(index == 1) info = "呵呵...";
                else if(index == 2) info = "Хех...";
            }
            if(info == "AkumaTransition2")
            {
                if(index == 0) info = "You know, kid...!";
                else if(index == 1) info = "你知道的, 小子...";
                else if(index == 2) info = "Знаешь, сопляк";
            }
            if(info == "AkumaTransition3")
            {
                if(index == 0) info = "The air around you begins to heat up...";
                else if(index == 1) info = "你周围的空气开始升温...";
                else if(index == 2) info = "Воздух вокруг вас нагревается";
            }
            if(info == "AkumaTransition4")
            {
                if(index == 0) info = "fanning the flames doesn't put them out...";
                else if(index == 1) info = "扇风可不能灭火...";
                else if(index == 2) info = "То, что ты подул на огни, не потушило их...";
            }
            if(info == "AkumaTransition5")
            {
                if(index == 0) info = "Akuma has been Awakened!";
                else if(index == 1) info = "邪鬼巨龙 已经觉醒!";
                else if(index == 2) info = "Акума пробудился!";
            }
            if(info == "AkumaTransition6")
            {
                if(index == 0) info = "IT ONLY MAKES THEM STRONGER!";
                else if(index == 1) info = "只 会 让 他 们 更 猛 烈!";
                else if(index == 2) info = "ЭТО ИХ ТОЛЬКО УСИЛИЛО!";
            }

            if(info == "AkumaA1")
            {
                if(index == 0) info = "Ashe? Help your dear old dad with this kid again!";
                else if(index == 1) info = "艾希? 再来帮你一次亲爱的老爸料理这小子!";
                else if(index == 2) info = "Аши? Помоги своему старому папе справиться с этим сопляком снова!";
            }
            if(info == "AkumaA2")
            {
                if(index == 0) info = "You got it, daddy..!";
                else if(index == 1) info = "懂了, 爸爸. . !";
                else if(index == 2) info = "У тебя получиться, папочка..!";
            }
            if(info == "AkumaA3")
            {
                if(index == 0) info = "Hey! Hands off my papa!";
                else if(index == 1) info = "嘿! 放开我爸!";
                else if(index == 2) info = "Эй! Убрал руки от моего папы!";
            }
            if(info == "AkumaA4")
            {
                if(index == 0) info = "Atta-girl..!";
                else if(index == 1) info = "艾女...!";
                else if(index == 2) info = "Моя девочка!";
            }
            if(info == "AkumaA5")
            {
                if(index == 0) info = "Still got it, do you? Ya got fire in your spirit! I like that about you, kid!";
                else if(index == 1) info = "明白吗? 你有火焰般炽热的精神! 小子, 我很喜欢这样!.";
                else if(index == 2) info = "Все еще жив, а? А в тебе есть стержень! Мне это нравится, сопляк!";
            }
            if(info == "AkumaA6")
            {
                if(index == 0) info = "What?! How have you lasted this long?! Why you little... I refuse to be bested by a terrarian again! Have at it!";
                else if(index == 1) info = "什么? !你怎么坚持这么长时间的? !为什么你这个小...我拒绝再被一个泰拉人打败! 攻击!";
                else if(index == 2) info = "Что?! Как ты держишься так долго?! Почему ты... Я отказываюсь быть побежденным террарианом снова! Получай!";
            }
            if(info == "AkumaA7")
            {
                if(index == 0) info = "ACK..! WATER! I LOATHE WATER!!!";
                else if(index == 1) info = "啊啊. . ! 水! 我讨厌水!!!";
                else if(index == 2) info = "АК..! ВОДА! Я НЕНАВИЖУ ВОДУ!!!";
            }
            if(info == "AkumaA8")
            {
                if(index == 0) info = "Nighttime won't save you from me this time, kid! The day is born anew!";
                else if(index == 1) info = "这次, 晚上可救不了你, 小子!已经是新的一天了! ";
                else if(index == 2) info = "В этот раз ночь тебя не спасет, сопляк! День перерождается!";
            }
            if(info == "AkumaA9")
            {
                if(index == 0) info = "You just got burned, kid.";
                else if(index == 1) info = "你烧伤了, 小子";
                else if(index == 2) info = "Ты только что сгорел, сопляк.";
            }
            if(info == "AkumaA10")
            {
                if(index == 0) info = "Heh, not too shabby this time kid. I'm impressed. Here. Take your prize.";
                else if(index == 1) info = "嘿, 小子这次很光明磊落. 我印象深刻. 这儿. 拿走你的战利品. ";
                else if(index == 2) info = "Хех, не такой убогий в этот раз, сопляк. Я впечатлен. Вот. Бери свой приз.";
            }
            if(info == "AkumaA11")
            {
                if(index == 0) info = "GRAH..! HOW!? HOW COULD I LOSE TO A MERE MORTAL TERRARIAN?! Hmpf...fine kid, you win, fair and square. Here's your reward.";
                else if(index == 1) info = "啊…!怎么回事!我怎么会输给一个普通的泰拉人? !嗯……好小子, 你赢了, 公平公正. 这是你的奖励. ";
                else if(index == 2) info = "ГРАХ..! КАК!? КАК Я МОГ ПРОЙГРАТЬ СМЕРТНОМУ ТЕРРАРИАНУ?! Хмпф... ладно, сопляк, ты выиграл честно. Вот твой приз.";
            }
            if(info == "AkumaA12")
            {
                if(index == 0) info = "Nice. You cheated. Now come fight me in expert mode like a real man.";
                else if(index == 1) info = "好好好. 你作弊. 你应该像一个真正的男子汉一样来专家模式挑战我. ";
                else if(index == 2) info = "Мило. Ты сжульничал. А теперь сразись со мной в эксперт моде, как настоящий мужчина.";
            }
            if(info == "AkumaA13")
            {
                if(index == 0) info = "Sky's fallin' again! On your toes!";
                else if(index == 1) info = "天又要塌了! 集中注意!";
                else if(index == 2) info = "Небеса вновь падают!";
            }
            if(info == "AkumaA14")
            {
                if(index == 0) info = "Down comes the flames of fury again!";
                else if(index == 1) info = "愤怒之火再次从天而降!";
                else if(index == 2) info = "И вот снова вниз спускаются огонь и ярость!";
            }
            if(info == "AkumaA15")
            {
                if(index == 0) info = "You underestimate the artillery of a dragon, kid!";
                else if(index == 1) info = "你低估了龙的火力, 小子!";
                else if(index == 2) info = "Ты недооцениваешь силу дракона, сопляк!";
            }
            if(info == "AkumaA16")
            {
                if(index == 0) info = "Flames don't give in till the end, kid!";
                else if(index == 1) info = "烈火永不熄灭, 小子!";
                else if(index == 2) info = "Огни не сдаются до самого конца, сопляк!";
            }
            if(info == "AkumaA17")
            {
                if(index == 0) info = "Heads up! Volcano's eruptin' kid!";
                else if(index == 1) info = "注意点! 火山喷发了, 小子!";
                else if(index == 2) info = "Осторожно! Вулканы извергаются снова";
            }
            if(info == "AkumaA18")
            {
                if(index == 0) info = "INCOMING!";
                else if(index == 1) info = "来了!";
                else if(index == 2) info = "СПАСАЙСЯ, СОПЛЯК!!";
            }
            if(info == "AkumaA19")
            {
                if(index == 0) info = "Hey Kid? Like Fireworks? No? Too Bad!";
                else if(index == 1) info = "嘿小子? 喜欢烟花吗? 不? 真糟糕!";
                else if(index == 2) info = "Эй, сопляк? Любишь фейерверки? Нет? Очень жаль!";
            }
            if(info == "AkumaA20")
            {
                if(index == 0) info = "Here comes the grand finale, kid!";
                else if(index == 1) info = "该完美收场了, 小子!";
                else if(index == 2) info = "А вот и твой грандиозный финал, сопляк!";
            }
            if(info == "AkumaA21")
            {
                if(index == 0) info = "The Sun won't quit 'til the day is done, kid!";
                else if(index == 1) info = "白昼不息烈日不止, 小子!";
                else if(index == 2) info = "Солнце не сдастся, пока день не закончиться, сопляк!";
            }
            if(info == "AkumaA22")
            {
                if(index == 0) info = "Face the fury of the sun!";
                else if(index == 1) info = "面对烈日的怒火吧!";
                else if(index == 2) info = "Сразись лицом к лицу с яростью солнца!";
            }

            if(info == "AkumaAAshe1")
            {
                if(index == 0) info = "Papa, NO! HEY! YOU! I'm gonna bake you alive next time we meet..!";
                else if(index == 1) info = "爸, 不! 啊! 你! 下次我们再见的时候, 我要把你活烤了. . !";
                else if(index == 2) info = "Папочка, НЕТ! ЭЙ! ТЫ! Я зажарю тебя в следующий раз..!";
            }
            if(info == "AkumaAAshe2")
            {
                if(index == 0) info = "OW, you Jerk..! I'm out!";
                else if(index == 1) info = "哦啊， 混蛋...! 我先撤了!";
                else if(index == 2) info = "АУ, урод..! Я все!";
            }

        }

        public static void AnubisChat(ref string info, int index)
        {
            if(info == "AnubisCombat1")
            {
                if(index == 0) info = "YEET";
                else if(index == 1) info = "咦嘻嘻！";
                else if(index == 2) info = "ЕЕЕЕЕ";
            }
            if(info == "AnubisCombat2")
            {
                if(index == 0) info = "Careful! Projectiles hurt.";
                else if(index == 1) info = "小心点! 我的射弹可有点疼.";
                else if(index == 2) info = "Осторожно! Снаряды бьют больно.";
            }
            if(info == "AnubisCombat3")
            {
                if(index == 0) info = "Let me grab some friends";
                else if(index == 1) info = "我叫点好兄弟来助助兴";
                else if(index == 2) info = "Давай я позову друзей.";
            }
            if(info == "AnubisCombat4")
            {
                if(index == 0) info = "Catch!";
                else if(index == 1) info = "接好!";
                else if(index == 2) info = "Лови!";
            }
            if(info == "AnubisCombat5")
            {
                if(index == 0) info = "Smashing, Init?";
                else if(index == 1) info = "压扁你的头, 然后咱们再来一遍？";
                else if(index == 2) info = "Сокрушительно, не так ли?";
            }

            if(info == "AnubisFalse")
            {
                if(index == 0) info = "HAH! Get hosed-- er, sanded.";
                else if(index == 1) info = "哈! 再回去练练吧-- 或者, 打磨一下自己.";
                else if(index == 2) info = "ХАХ! Кого теперь облили вод-песком!";
            }

            if(info == "AnubisGuys")
            {
                if(index == 0) info = "guys";
                else if(index == 1) info = "伙计们";
                else if(index == 2) info = "ребят";
            }
            if(info == "Anubisbud")
            {
                if(index == 0) info = "bud";
                else if(index == 1) info = "哥们";
                else if(index == 2) info = "дружок";
            }

            if(info == "Anubis1")
            {
                if(index == 0) info = "Well, ";
                else if(index == 1) info = "好, ";
                else if(index == 2) info = "Что же, ";
            }
            if(info == "Anubis2")
            {
                if(index == 0) info = ". Here we are.";
                else if(index == 1) info = ". 到地方了.";
                else if(index == 2) info = ". вот и тот самый момент.";
            }
            if(info == "Anubis3")
            {
                if(index == 0) info = "I hope you're ready for a real fight.";
                else if(index == 1) info = "我希望你已经对真正的战斗做好了充足的准备.";
                else if(index == 2) info = "Я надеюсь, ты готов к настоящему бою.";
            }
            if(info == "Anubis4")
            {
                if(index == 0) info = "Especially since I'm in my superior form.";
                else if(index == 1) info = "尤其是因为我现在处于我的超强形态.";
                else if(index == 2) info = "Учти, что я в своей самой сильной форме";
            }
            if(info == "Anubis5")
            {
                if(index == 0) info = "You ready? I won't hesitate to slap you silly!";
                else if(index == 1) info = "准备好了吗? 我这一大嘴巴子下去可不会手下留情!";
                else if(index == 2) info = "Ты готов? Я не буду стесняться бить тебя!";
            }
            if(info == "Anubis6")
            {
                if(index == 0) info = "Let's go!";
                else if(index == 1) info = "开始吧!";
                else if(index == 2) info = "Поехали!";
            }
            if(info == "Anubis7")
            {
                if(index == 0) info = "A rematch eh? Alright, this should be fun!";
                else if(index == 1) info = "再比一次，嗯? 好呀, 想必很有乐趣!";
                else if(index == 2) info = "Хочешь реванш, а? Ладненько, будет весело!";
            }

            if(info == "AnubisTransition1")
            {
                if(index == 0) info = "...hrgh...";
                else if(index == 1) info = "...呼哈...";
                else if(index == 2) info = "... хргх...";
            }
            if(info == "AnubisTransition2")
            {
                if(index == 0) info = "...alright.";
                else if(index == 1) info = "...好了.";
                else if(index == 2) info = "... ладно.";
            }
            if(info == "AnubisTransition3")
            {
                if(index == 0) info = "I think...it's time.";
                else if(index == 1) info = "我觉得...是时候了.";
                else if(index == 2) info = "Я думаю... что время пришло.";
            }
            if(info == "AnubisTransition4")
            {
                if(index == 0) info = "No more stops being pulled.";
                else if(index == 1) info = "有些事情已经无法阻止了.";
                else if(index == 2) info = "Больше никаких детских игр.";
            }
            if(info == "AnubisTransition5")
            {
                if(index == 0) info = "If you're gonna be taking on the dark forces of the world...";
                else if(index == 1) info = "如果你想要和这个世界上的黑暗势力搏杀的话...";
                else if(index == 2) info = "Если ты будешь сражаться с темными силами этого мира...";
            }
            if(info == "AnubisTransition6")
            {
                if(index == 0) info = "I need to make sure you're ready, because...unless you're ready...";
                else if(index == 1) info = "我就必须确认你已经完全准备好了, 因为...倘若你还没准备好...";
                else if(index == 2) info = "Мне надо быть уверенным, что ты готов... потому что если ты не готов...";
            }
            if(info == "AnubisTransition7")
            {
                if(index == 0) info = "...Some things should stay locked away for your own good.";
                else if(index == 1) info = "...为你好，有些东西你还是不碰为妙.";
                else if(index == 2) info = "... некоторые вещи должны быть скрыты от тебя";
            }

            if(info == "FAnubisCombat")
            {
                if(index == 0) info = "No Warnings this time.";
                else if(index == 1) info = "这次可没有预警.";
                else if(index == 2) info = "В этот раз без предупреждений.";
            }

            if(info == "FAnubisWin")
            {
                if(index == 0) info = "...You done good, bud. Let's make a game plan moving forward. Come talk to me when you're ready.";
                else if(index == 1) info = "...你做的很好，兄弟. 让我们来制定一个前进的计划，准备好了就来找我.";
                else if(index == 2) info = "... Ты справился хорошо, приятель. Давай продвинемся дальше. Поговори со мной, когда будешь готов..";
            }
            if(info == "FAnubisLose")
            {
                if(index == 0) info = "...Sorry, but you aren't ready yet.";
                else if(index == 1) info = "...对不起, 看来你还没准备好.";
                else if(index == 2) info = "... Извини, ты еще не готов.";
            }
        }

        public static void RajahChat(ref string info, int index)
        {
            if(info == "SupremeRajahChat")
            {
                if(index == 0) info = "Rajah glows with furious energy as he attacks, strengthening his defenses";
                else if(index == 1) info = "王公兔在攻击时散发巨大的愤怒能量, 强化了他的防御";
                else if(index == 2) info = "Атакуя, Раджа горит яростной энергией, усиливая свою защиту";
            }
            if(info == "SupremeRajahChat2")
            {
                if(index == 0) info = "MUDERER";
                else if(index == 1) info = "杀 人 犯";
            }
            if(info == "SupremeRajahChat3")
            {
                if(index == 0) info = "Terrarians";
                else if(index == 1) info = "你们泰拉人";
            }
            if(info == "SupremeRajahChat4")
            {
                if(index == 0) info = "THIS ISN'T THE END, ";
                else if(index == 1) info = "这 事 没 完, ";
                else if(index == 2) info = "ЭТО ЕЩЕ НЕ КОНЕЦ, ";
            }
            if(info == "SupremeRajahChat5")
            {
                if(index == 0) info = "! RIVALS CLASH TILL THE VERY END!";
                else if(index == 1) info = "!我 会 打 到 最 后!";
                else if(index == 2) info = "! СОПЕРНИКИ СРАЖАЮТСЯ ДО КОНЦА!";
            }
            if(info == "SupremeRajahChat6")
            {
                if(index == 0) info = "And stay down.";
                else if(index == 1) info = "给我跪下. ";
                else if(index == 2) info = "И не вылезай.";
            }
            if(info == "SupremeRajahChat7")
            {
                if(index == 0) info = "Coward.";
                else if(index == 1) info = "懦夫. ";
                else if(index == 2) info = "Трус.";
            }
            if(info == "SupremeRajahChat8")
            {
                if(index == 0) info = "Well fought, ";
                else if(index == 1) info = "打得不错, ";
                else if(index == 2) info = "Неплохо сражался, ";
            }
            if(info == "SupremeRajahChat9")
            {
                if(index == 0) info = ". Take your reward.";
                else if(index == 1) info = ". 拿着你的奖励. ";
                else if(index == 2) info = ". Бери награду.";
            }
        }
    }
}