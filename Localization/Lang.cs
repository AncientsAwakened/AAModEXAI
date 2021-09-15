using Terraria.Localization;

namespace AAModEXAI.Localization
{
    public class Trans
    {
        // 0 is English
        // 1 is Chinese
        // 2 is Russian
        public static string  text(string place, string info)
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

            if(place == "Common") Common(ref info, index);
            if(place == "AH") AHChat(ref info, index);
            return info;
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
    }
}