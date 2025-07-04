﻿Console.OutputEncoding = System.Text.Encoding.UTF8;
string SanitizeTextInput(string input, string fallback, bool integerMode)
{
    input = input.Trim();
    if (input == null) return fallback;
    else if (input.Length == 0) return fallback;
    else if (integerMode && !Int32.TryParse(input, out _)) return fallback;
    else return input;
}
int numberOfPlayers;
string victim = string.Empty;
string killer = string.Empty;
string bystander = string.Empty;
const int deathChance = 60; // Percentage chance of a death message being displayed
string logFileName = $"battlelog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);
List<string> logLines = new List<string>();
void WriteMessage(string input)
{
    Console.WriteLine(input);
    logLines.Add(input);
}
string[] deathMessages =
    [
        "{0} was killed by {1}. ",
        "{0} wanted to send a 'send nudes' message to {1}, but {0} mistyped it as 'send nukes'. ",
        "{0} wanted to see {1}'s pet. What they didn't know is that {1}'s pet was Something Unto Death (nicknamed 'Sleepie'). ",
        "{0} wanted to see {2}'s pet. What they didn't know is that {2}'s pet was {1}, who was really feeling like killing {0}. ",
        "{0} was shot by {1} who was shot by {0}'s twin brother who was shot by the real {1} (as the previous {1} was a decoy). ",
        "{0} and {1} summoned The Twins together. However, they both forgot they had PvP turned on, and {1} accidentally shot {0}. ",
        "{0} and {1} got into an utensil battle. {0} forgot that {1} is a freak and considers a gun as an utensil. ",
        "{0} and {1} got into an utensil battle. {0} forgot that {1} is a freak and considers a sword as an utensil. ",
        "{0} and {1} got into an utensil battle. {0} forgot that {1} is a freak and considers a spork as an utensil. ",
        "{0} was shrunk by {1} into a miniature size, and then {1} released a ravenous octopus who ate {0} whole like a weird sardine. ",
        "{0} was the man who fell into the river in LEGO City. And {1} procrascinated on building the rescue helicopter. ",
        "{0} played Rock-Paper-Scissors against {1}. {0} used Rock. {1} used Shotgun. ",
        "{0} played Rock-Paper-Scissors against {1}. {0} used Paper. {1} used Shotgun. ",
        "{0} played Rock-Paper-Scissors against {1}. {0} used Scissors. {1} used Shotgun. ",
        "{0} said 'Kill me please', and the wish got answered by {1}. ",
        "{0} joined {1}'s Literature Club. A few days later, {1} deletes '{0}.chr'. ",
        "{0} killed {1}'s dog. {1} retaliated. ",
        "{0} swapped minds with {1}. {1} foresaw that and poisoned themselves beforhand. ",
        "{0} deemed themselves the Chaos King. {1} dealt with that information like the French (aka. with a guillotine). ",
        "{0} went to {1}'s party. {1} knew that it was {0}'s last party, before killing them with a sawblade. ",
        "{0} came to be the Werewolf. {1} was prepared with inhuman amounts of silver in their posession. ",
        "{0} wanted to fight {1}, but {1} stopped time, filled {0}'s stomach cavity with their knives, and then resumed time. ",
        "{0} started dancing the 'Shikanoko nokonoko koshitantan' dance. {1} failed to prevent themselves from killing {0}. ",
        "{0} dropped Yoshi off a cliff into a bottomless pit. {1} decided to do the same with {0}. ",
        "{0} thought that this battle was easy breezy. {1} proved them wrong. ",
        "{0} went on a date with {1}, only to find out that they were a Ghoul. ",
        "{0} tried to escape {1}. ",
        "{0} fought {1}, but the turn limit struck 0. ",
        "{0} accidentally overrided themselves with {1}. ",
        "{0} wanted to do a Midnight Parade, but {1} got into the Rumia persona a little bit too well and devoured {0}. ",
        "{0} considered themselves as an Internet Angel, but {1} gave them a reality check, showing that {0} is only a needy streamer on an internet overdose and with a failing career. ",
        "{0} dared to eat {1}'s pudding. ",
        "{0} refused to listen to {1}. ",
        "{0} got got by {1}. ",
        "{0} woke up {1} from their nap so hard {1} one-shot {0}. ",
        "{0} was busy typing up commands and trying to spawn in rare items, but they were interrupted by {1}. Their last words were: \"/give {0} diamonsss aaaaaaaaaaa\". ",
        "{0}, {1}, and {2} did a strength contest. {2} picked up a car. {1} picked up a building. {0} died of overextertion trying to pick up a water bottle. ",
        "{0}, {1}, and {2} played a battling game, but had only 2 controllers. Instead of doing a little tournament, {1} decided to solve the issue by killing {0}. ",
        "{0}, {1}, and {2} played Among Us together. {0} was so sus that {1} ejected them out of the window, then lost because {2} was the Impostor. ",
        "{0} and {1} fought together, while {2} jokingly played the Mortal Kombat theme. In the end, {1} performed a Fatality on {0}. ",
        "{0}, {1}, and {2} played Shiritori together. {0} ended the game with the word 'ramen'. ",
        "{0}, {1}, and {2} played Shiritori together. {0} ended the game with the word 'udon'. ",
        "{0}, {1}, and {2} played Shiritori together. {0} ended the game with the word 'Ramadan'. ",
        "{0}, {1}, and {2} played Shiritori together. {0} ended the game with the word 'Dandadan'. ",
        "{0}, {1}, and {2} played Shiritori together. {0} ended the game with the word that {1} started with. ",
        "{0} accidentally threw a Wiimote straight into {2}'s TV, so {1} 'accidentally' threw a Wiimote straight into {0}'s skull. ",
        "{0} and {2} battled {1}, Mario & Luigi style. {1} defeated {0}, and {2} remembered that they forgot to bring any 1-up mushrooms. ",
        "{0} swapped minds with {2}, oblivious to the fact that {1} planted a bomb on {2}. ",
        "",
        "{0} skill issued. ",
        "{0} said the N-word and was kicked out of the game. ",
        "{0} forgot that they set the game to Hardcore Mode, and tried to restore their hunger by falling off a cliff near their base. ",
        "{0} forgot to never dig straight down a 1x1 tunnel, and they dug straight into a lava pool. ",
        "{0} was false-banned because the admin said so. ",
        "{0} tried to play Touhou on a NerveGear. They forgot that they suck at bullet hell games. ",
        "{0} did an IQ quiz with Cirno. Cirno got a higher score than {0}, so {0} literally imploded from embarassement. ",
        "{0} fought Aventurine. With only single-target units. ",
        "{0} fought Sans, and for some reason really felt their sins crawling on their back. ",
        "{0} entered this battle with only underwear. ",
        "{0} got a sudden heart attack. ",
        "{0} watched an 'Do not laugh challenge' video. They laughted. ",
        "{0} mondegreened and sung 'I'm blue da ba dee da ba daa' as 'I'm blue, if I was green I would die'. They forgot that they dyed their hair green recently. ",
        "{0} saw an mysterious song that appeared on their playlist on its own. It was somehow a mimic. ",
        "{0} fell into a well and waited for help, completely oblivious of the fact that their real name is 'Jugemu Jugemu Goko no Surikire Kaijasugiyo no Suigyomatsu Unraimatsu Furaimatsu Ku Neru Tokoro ni Sumu Tokoro Yabura Koji no Bura Koji Paipo-paipo Paipo no Shuringan Shuringan no Gurindai Gurindai no Ponpokopi no Pompokona no Chokyumei no Chosuke'. ",
        "{0} played a marathon beatmap with the Perfect mod. They choked at the last circle. ",
        "{0} watched so much brainrot that their brain literally rot. ",
        "{0} lost a Rock-Paper-Scissors game against a mirror. ",
        "{0} decided to give Technoblade a visit. ",
        "{0} hit a punching bag as hard as they could muster. The punching bag hit {0} back. ",
        "{0} was killed by {0}. ",
        "{0} left the game. ",
        "{0} waited so long for their turn to battle that they actually died of old age. ",
        "{0} played an extremely difficult beatmap with the HRPFNCHDFL mod combo. It was so stressfull that their heart wasn't able to survive the adrenaline. ",
        "{0} fell into their own creation, and shattered across time and space. ",
        "{0} dressed into Senketsu for the battle. Senketsu was incredibly hungry today, and sapped all of {0}'s blood. ",
        "{0} went to the city square. An hour later, they got cooked by the heatwave. ",
        "{0} lost all 4 of their lives in the Prime Empire. ",
        "{0} perished to Ligma. ",
        "{0}'s clock reached 12, and as such they were sent to the deepest parts of the Abyss. ",
        "{0} thought they were Spider-Man. They were not, as their remains on the pavement prove. ",
        "If this is Chapter 9, then {0} is Himeko. In other words, ",
        "{0} played the War stage in Pizza Tower, and timed out. What they didn't notice was that they had a real bomb connected to their PC. ",
        "{0} failed to escape a crumbling tower in time. ",
        "{0} wanted to make some cereal. Not even 10 minutes later and they burned down their entire house, themselves included. ",
        "{0} smoked a cigarette. ",
        "{0} was sent to Brazil. ",
        "{0} became so famous that people started writing fanfictions about them. Out of curiosity, they read one and immediately died of embarrassment. ",
        "{0} tried to play Rush E on their personal piano, and mangled their own hands beyond recognition. ",
        "{0} got straight-up deleted. ",
        "{0} sacrificed themselves. ",
        "Hey {0}, are you worried that you'll die in this scene? You're right! ",
        "{0} took on a special challenge: to take a sip of vodka for every terrible meme/reference they saw in an epic battle scene. ",
        "{0} drank a cistern's worth of sake in one go, and as they aren't an Oni, their liver literally exploded. ",
        "{0} fought a ghost version of themselves... and lost. ",
        "{0} got blown away by the wind. ",
        "Kogasa Tatara surprised {0} so hard that they died of a heart attack. ",
        "{0} became a truly valiant hero. But at what cost? Well, ",
        "{0} played RUSH E so well that their personal piano catched on fire. ",
        "Apparently, {0} was a diabetic, and just died of diabetes. ",
        "Apparently, {0} was a diabetic, and just died of hyperglycaemia. ",
        "Apparently, {0} was a diabetic, and just died of hypoglycaemia. ",
        "{0} activated their cheats. They were immediately banned. ",
        "{0} turned into stone. ",
        "{0} ate a Chemical Cookie done in a Magical Mixer. It had POISON inside, and the YUC'e reference overload doesn't help. ",
        "{0} died from an acquired poison. ",
        "{0} died from an internet overdose. ",
        "{0} underestimated K. Round's strength. ",
        "{0} was killed by Truck-kun. ",
        "{0} watched one too many Japanese ads and lost it from the absurdity. ",
        "{0} fought The Devourer. With only Bio attacks. ",
        "{0} wanted to see the mermaids so badly they didn't prepare anything to actually breathe while underwater. ",
        "{0}'s last words: \"Uh oh.\" ",
        "{0}'s last words: \"OH MY GOD PLEASE NOOOOOO-\" ",
        "{0}'s last words: \"Shikanoko nokonok-\" ",
        "{0}'s last words: \"Touka spadła ze stołk-\" ",
        "{0} had the audacity to generate an AI image. ",
        "{0} did a jetpack joyride... and flew straight into a laser. ",
        "{0} got eaten by Jožin z Bažin. ",
        "{0} suddenly realised that they are a character from Amphoreus. ",
        "{0}'s time ran out. ",
        "{0} got blown away by the wind. ",
        "{0} tried to do the Freedom Dive... and failed. ",
        "{0} entered the Suicide Parade. ",
        "{0} was sent to the Abyss. ",
        "{0} has a rave in their own grave. ",
        "{0} ragequit. ",
        "{0} couldn't run away from the fact that the world tries to kill them. ",
        "{0} became glitched beyond recognition. ",
        "Eventually, {0} stopped thinking. ",
        "404 ERROR: {0} not found. ",
        "{0} listened to a song so fire they spontaneously combusted. ",
        "{0} tried to cook. And detonated the whole kitchen. ",
        "{0}'s canteen ran empty, so they quenched their thirst with the liquid powering their glowstick. ",
        "{0} drank dihydrogen monoxide. ",
        "{0} drank dihydrogen peroxide. ",
        "{0} entered the Absolutely Safe Capsule. ",
        "{0} took out a family photo and started talking about how they couldn't wait to get home. This was an obvious death flag. ",
        "You wouldn't believe what happened to {0}! ",
        "{0} stopped time. Including for themselves. ",
        "{0} got embraced by the flame. Literally. ",
        "SERN decided to send {0} into the past as a jellyman. ",
        "{0} jumped out of a ground floor window. ",
        "{0} was sent to Radom. ",
        "{0} drank Desderman. ",
        "{0} drank Domestos. ",
        "{0} drank {0}. ",
        "{0} ate {0}. ",
        "{1} summoned Beatrice to take care of {0}. ",
        "{1} killed {0}. ",
        "{1} hacked all the nuke launchers in the world and sent all the nukes straight onto {0}'s house. ",
        "{1} was supposed to be killed by {0}, but {1} pulled out an Uno Reverse in the last possible moment. ",
        "{1} was supposed to be killed by {0}, but it came out that {1} was an Amanojaku. ",
        "{1} unleashed their inner Rumia and devoured {0} whole. ",
        "{1} performed a pretty little massacre and {0} was unluckily passing through nearby. ",
        "{1} unleashed their inner Magda Gessler at {0}'s restaurant and threw a plate right into {0}'s nape. ",
        "{1} wanted to do a skateboard trick, while suddenly they crash into {0}. {1} left the scene unscatched, while {0} had all of their bones shattered. ",
        "All that flailing with a hammer caused {1} to accidentally smash {0}'s head. ",
        "{1} strapped {0} to a firework and watched them fly. ",
        "{1} threw the Guide Voodoo Doll into Underworld's lava to summon the Wall of Flesh, killing {0} the Guide. ",
        "{1} wanted to roleplay a dictator, by sending {0} to a Gulag. ",
        "{1} decited to relocate {0} to 'very fucking far away'. ",
        "{1} used {0} as a sacrifice to the Blood God. ",
        "{1} made {0}'s name the Lost Word. ",
        "Nobody survives {1}'s balls. {0} learnt that the hard way. ",
        "{1} was killed by {0}. Wait, it is the other way around! ",
        "{1} entered Overkill Mode... and reduced {0} to individual quarks. ",
        "{1} sang 'Stronger Than You' to {0}. Later, {0} learned the hard way that {1} WASN'T lying in that they're stronger. ",
        "{1} shot {0} straight into a black hole. ",
        "{1} surprised {0} so hard that {0} got a heart attack. ",
        "{1} HANIPAGANDA'd {0}. ",
        "{1} decided to recreate Bad Apple using {0}'s incestines. ",
        "{1} lost control over their own gun, accidentally killing {0}. That's why you should disable friendly fire! ",
        "{1} is an angel with a shotgun. {0} learnt that the hard way. ",
        "{1} tore off some petals of a rose, not knowing that it represents {0}'s health. ",
        "{1} was roleplaying an executioner. By the time {0} came to them, {1} forgot that it was supposed to be a roleplay. ",
        "{1} gave {0} a bad time. ",
        "{1} left their Bleach at a shelf. {0} got curious and drank it. ",
        "{1} is so good at bowling that they got a max score with a HEAD. To be more precise, {0}'s head. ",
        "{1} committed a senseless massacre, and {0} was one of its victims. ",
        "{1} found out that they have the power to propel a coin like a railgun from their own fist. They used {0} as their target. ",
        "{1} sent {0} to Brazil. ",
        "{1} was hired by the SCP Foundation to take care of SCP-{0}. ",
        "{1} called {0} a 'boomer', which caused them to literally explode. ",
        "{1} strongly warned {0}, and this time it was not a joke. ",
        "{1} invoked a magical storm, turning {0} inside-out. Quite literally. ",
        "{1} made {0} a nice 'paradise hell', which was too much for {0}. ",
        "{1} squished {0} so hard that they went below their Schwarzschild radius. ",
        "{1} unintentionally killed {0}, but they can't use SAVEs to revert the situation. ",
        "{1} somehow quantum entangled {0} with a lit bomb. ",
        "{1} threw {0} through a mirror, and then shattered it. ",
        "{1} absolutely obliterated {0}. ",
        "{1} revealed their vampiric nature, and sucked all of {0}'s blood. ",
        "{1} started a nuclear reactor. Unfortunately, {0} was doing a routine mainetance, and got very radiated. ",
        "{1} overrode {0}'s data with junk data, trying to make a 'glitch player'. It failed, and ",
        "{1} decided to play as Dr. Primitive, and turned {0} into a monkey. ",
        "{1} decided to play as Dr. Primitive, and turned {0} into a banana. ",
        "{1} got inspirted by Calamitas and burned down {0} like they were Azafure or something. ",
        "{1} got inspirted by Calamitas and burned down {0} like they were Ilmeris or something. ",
        "{1} got into their hidden Facist side and killed {0} without a single bit of remorse. ",
        "{1} killed {0} so hard that they got Discord banned in the country. ",
        "{1} made {0} into a 'kangaroo'. ",
        "{1} made {0} walk the plank. ",
        "{1} utterly massacred {0}... in this game. ",
        "{1} utterly massacred {0}... in real life. ",
        "{1} utterly massacred {0}... in general. ",
        "{1} threw {0} into a gap that's definitely there. ",
        "{1} decided to become a cannibal, and ate {0} alive. ",
        "{1} sent {0} to the past using a microwave. ",
        "{1}: Omae wa mou shindeiru! {0}: NANI?! ",
        "{1} snapped {0}'s neck. ",
        "{1} rickrolled {0}. ",
        "{1} Tanaka'd the Tanaka out of {0}'s Tanaka using {2}'s Tanaka Katana. ",
        "{1} sacrificed {0} to get {2}'s blessings. ",
        "{1} started the Watanagashi festival. As per tradition, {0} was found dead, while {2} is missing. ",
        "{1} and {2} wanted to kill this love, by killing {0}. ",
        "{1} and {2} found {0}'s corpse at an abandoned school in another dimension. ",
        "{1} used {2} as a weapon to kill {0}. ",
        "{1} and {2} tapped into the abilities of the Crazy Backup Dancers (Mai Teireida and Satono Nishida), and started dancing behind {0}. Because {1} got Mai's ability, it was them who took all of {0}'s vitality. ",
        "{2} pissed off {1} so much that they did a tableflip straight onto {0}, crushing them with the sheer force of a table. ",
        "[this death message was censored] "
    ];
string[] fillerMessages =
    [
        "{0} is idle.",
        "{0} is sick of all the bloodshed.",
        "{0} hopes they can survive the next message.",
        "{0} blissfully listens to their favourite music.",
        "{0} had a nightmare about being killed by {1}.",
        "{0} doesn't know what to do.",
        "{0} decided to take a power nap.",
        "{0} is afraid of what the enemies will do.",
        "{0} wonders if they will win this game.",
        "{0} wonders why they lose.",
        "{0} wonders why. {0} wonders how.",
        "{0} wonders who will kill {1}.",
        "{0} wonders if they can explain something without the word 'Destruction'.",
        "{0} wonders about the fate of those who lose in this battle.",
        "{0} wonders if they could make Bad Apple in a command prompt.",
        "{0} wonders if they could resurrect themselves after dying.",
        "{0} wonders if this is how the world ends.",
        "{0} accidentally threw their favourite T-shirt into a can of paint. {0} dyed.",
        "{0} played some Touhou on the Lunatic difficulty, and now they feel like they're better at dodging attacks.",
        "{0} screamed a total of 9001 times to enter the Super form. {1} did the same by eating a mushroom.",
        "{0} thought about The Game, so they lost The Game, and now you too lost The Game.",
        "{0} started dancing the 'Shikanoko nokonoko koshitantan' dance. {1} has to heavily force themselves not to kill them.",
        "{0} already prepares their very own burial.",
        "{0} has lost their umbrella, and now they're frantically searching for it.",
        "{0} thinks that it's a good time for a fiesta.",
        "{0} starts preparing for their next kill.",
        "{0} tries to escape this game. Coward.",
        "{0} dances til' their death, or at least they want to do so.",
        "{0} drank a freshly brewed Obsidian Skin Potion, and took a relaxing swim in a pool of lava.",
        "{0} asked {1} the question of 'Excuse my rudeness, but could you please RIP?<3'.",
        "{0} starts unleashing their inner Satan.",
        "{0} drank too much, thankfully not enough for their liver to fail. Just enough for them to be drunk like a wheelbarrow.",
        "{0} confessed to {1}. I honestly wanna see how this will go.",
        "{0} tries to defeat the heatwave.",
        "{0} honestly thinks that it's the Ninth Circle of Hell or something.",
        "{0} somehow missed their delayed train.",
        "{0} just joined the Envoys of the Kingslayer. All they can say with their hacking skills is that {1} will likely die.",
        "{0} thinks that it's enough, and tries to leave, without any results.",
        "{0} asked the real question of if U.N. Owen Was really Her.",
        "{0} decided that it's time to just go and say 'Let's rock it!'.",
        "{0} has a sudden urge to eat spaghetti, make puzzles or capture humans. Maybe all three.",
        "{0} started playing SMB out of boredom.",
        "{0} thinks on how they are going to kill next time.",
        "{0} started singing about bamboo, startling everyone.",
        "{0} says that no matter how they look at it, it's {1}'s fault that they're not popular.",
        "{0} is feeling like reading a ghost story.",
        "{0} has a sense of Deja Vu. Were there any repeated messages?",
        "{0} decided to join the Idolatrising Warriors. Now Kasumi is their idol.",
        "{0} wanted to close their 3rd eye. Then they remembered that they weren't a Satori.",
        "Everyone wonders if {0} is an alien or not.",
        "{0} seems to like that one android girl.",
        "{0} just bought a 'Welcome to Hell' shirt from Hecatia.",
        "{0} looked at the black mirror on the wall, thinking about their friends that have fallen in this battle.",
        "{0} thinks that they found their place.",
        ".ecnetnes siht deppilf {0}",
        "{0} starts apologizing to everyone.",
        "{0} starts a weird-looking ritual.",
        "{0} unleashes their inner animal.",
        "{0} daydreams about having Sakuya's power to manipulate time in order to stop time at will.",
        "{0} contemplates their life choices, getting an intrusive thought of suicide as a result.",
        "{0} wants to name their future child 'Jugemu Jugemu Goko no Surikire Kaijasugiyo no Suigyomatsu Unraimatsu Furaimatsu Ku Neru Tokoro ni Sumu Tokoro Yabura Koji no Bura Koji Paipo-paipo Paipo no Shuringan Shuringan no Gurindai Gurindai no Ponpokopi no Pompokona no Chokyumei no Chosuke'.",
        "{0} thinks that right now it's their best nightmare.",
        "{0} startles everyone by making a LEGO Yoda scream.",
        "{0} thinks that this battle is a punishment for something.",
        "{0} tries to resurrect someone who already died in this battle. It naturally fails.",
        "{0} thinks that they saw their own doppelganger.",
        "{0} shouts for others to make some noise.",
        "{0} starts to doubt the point of this battle.",
        "{0} and {1} think about their time spent together.",
        "{0} considers {1} a smooth criminal.",
        "{0} is on a kill streak that's describable with a natural number.",
        "Remember {0}'s and {1}'s romance? No? Well, it was a fake love, so nothing worth remembering.",
        "{0} thinks that {1} is a monster.",
        "{0} thinks that {1} is a liar.",
        "{0} wants to go whenever, wherever - just to not be here right now.",
        "{0} knows that it's 'Kill or be killed'.",
        "{0} knows that it's 'No pain, No game'.",
        "{0} has some words that they never said to their fallen friend.",
        "{0} read {1}'s mind. A foolish mistake, as {0} saw things that they definitely didn't want to see.",
        "{0} prepares their last resort.",
        "{0} sang '{1}, baby please don't make me cry, tylko buzi daj, buzi daj'.",
        "{0} honestly ships {1} and {2} together.",
        "{0} unironically ships themselves with {1}.",
        "{0} organised a spelling bee! {1} got 'apple'. {2} got 'supercalifragilisticexpialidocious'.",
        "{0} organised a spelling bee! {1} got 'bird'. {2} got the full name of Titin.",
        "{0} challenged {1} to count up to five quadrillion.",
        "{0} challenged {1} to bring them the stone begging bowl of the Buddha.",
        "{0} challenged {1} to bring them a jeweled branch from the island of Hourai.",
        "{0} challenged {1} to bring them a robe made from the skin of a fire rat.",
        "{0} challenged {1} to bring them a coloured dragon jewel.",
        "{0} challenged {1} to bring them a cowry shell born from a swallow.",
        "{0} is trying to solve a crossword puzzle.",
        "{0} is trying to solve a jigsaw puzzle.",
        "{0} asked {1} if they could help with dealing with {2}.",
        "{0} wishes the messages would go a little faster.",
        "{0} decided to do a little disco!",
        "{0} believes that the biggest enemy of justice is justice itself.",
        "{0} thinks about their next big-brain play.",
        "{1} killed {0}... only for a smaller {0} to come out of them, like a Matryoshka doll.",
        "{0} asked {1} 'What was brought into being after we hurt each other?'. {1} had an excellent answer; dystopia.",
        "{0} asked {1} 'Hand in hand, what did we protect?'. {1} had an excellent answer; dystopia.",
        "{0} starts dancing the Crab Rave.",
        "{0} went mad like a hatter.",
        "{0} went onto a diet. It completely changed their look, making them look almost like {1}'s doppelganger.",
        "{0} wants to die.",
        "{0} wants {1} to tell them if they can save that shit.",
        "{0} told {1} that they suck at love.",
        "{0} wants a redo of this whole battle.",
        "{0} wants to showcase their assassination skills they learnt in school.",
        "{0} hopes that they reincarnate as a slime after they die.",
        "{0}: {1} Get The Banana\n{0}: Potassium",
        "{0} thinks that 'Happppy Song' by SOOOO would be a good background theme for this battle.",
        "{0} tried to make a great escape. Naturally, they failed.",
        "{0} had made a truce with {1}.",
        "{0}, {1}, and {2} made a triple truce with each other.",
        "{0} counts up to 99 in hope to activate their true power.",
        "{0} trusts in eternity, and that time is eternal.",
        "{0} gives some choccy milk to {1}.",
        "{0} wants to be a girl. Or maybe they're already a girl?",
        "{0} wants to be a boy. Or maybe they're already a boy?",
        "{0} wants to be a hermaphrodite. Or maybe they're already a hermaphrodite?",
        "{0} wants to be a cat. Or maybe they're already a cat?",
        "{0} made a deal with the Devil.",
        "{0} made a deal with {1}.",
        "{0} said to {1} 'Well, Just You Wait!'.",
        "{0} feels like the Lord of the Game.",
        "{0}: Nah, I'd win",
        "{0} just got a new highscore!",
        "{0} wonders how did they become a god.",
        "{0} goes back and forth, spreading miracles everywhere and reincarnating others everyday.",
        "{0} told others to not mind them.",
        "{0} sings the Ievan Polkka.",
        "{0} declared themself as the Mr. Fanservice.",
        "{0} has a dillema in terms of who to kill.",
        "{0} started to wonder if their life was just a sequence of random events that have the possibility of killing them.",
        "{0} considers it their own duty to survive.",
        "{0} just got a 5* character from a gacha: {1}. It was the limited banner character.",
        "{0} just got a 5* character from a gacha: {1}. {0} lost the 50/50, but still got something nice.",
        "{0} just got a 5* character from a gacha: {1}. {0} lost the 50/50 so hard that {1} just had to say 'Priviet!'.",
        "{0} just got a 4* character from a gacha: {1}. It is pretty damn good!",
        "{0} just got a 4* character from a gacha: {1}. It is okay-ish.",
        "{0} just got a 4* character from a gacha: {1}. It is Arlan-tier.",
        "{0} employed some Tsukumogami to make some background music. Too bad it's a console app.",
        "{0} strongly warns {1}.",
        "Wonder how toxic would a ship of {0} and {1} would be?",
        "{0} decided to have a feast with {1} and {2}.",
        "{0} turned to hedonism in these trying times.",
        "{0} starts imagining this combat as being alike to EBF5, despite this being absolutely not the case.",
        "{0} starts imagining this combat as being alike to HSR, despite this being absolutely not the case.",
        "{0} starts imagining this combat as being alike to Terraria, despite this being absolutely not the case.",
        "{0} starts imagining this combat as being alike to Minecraft, despite this being absolutely not the case.",
        "{0} starts imagining this combat as being alike to Genshin, despite this being absolutely not the case.",
        "{0} starts imagining this combat as being alike to WuWa, despite this being absolutely not the case.",
        "{0} starts to do a nice disco with some monkeys.",
        "{0} wants to turn up the difficulty mode to 'Hellish Paradise'. It's not that game, sorry.",
        "{0} starts behaving like an animal from boredom.",
        "{0} and {1} make a perfect team! Probably. Perhaps. Maybe. I don't know. They might even kill each other.",
        "{0} takes a nap.",
        "{0} met their demise after being fatally killed to death. Somehow, the quadruple-negative cancelled itself out, and {0} ended up completely fine.",
        "{0} thinks that this game is perfectly balanced and with no exploits.",
        "{0} had the balls to tell {1} that they absolutely suck at this game.",
        "{0} wants to be a [[BIG SHOT]].",
        "{0} killed {1}. Thankfully, {1} still has some lives left.",
        "{0} sneaked into the reward room and took all the rewards for themselves.",
        "{0} has a weather report: 'it fuckin windy'.",
        "{0} dances the Third Eye Tango.",
        "{0} tries to become invisible. It failed.",
        "{0} tries to become invisible. They became slightly transparent for a moment.",
        "{0} tries to invoke their Alter Ego for the battle.",
        "{0} looted absolutely everything that {1} had.",
        "{0} activated their cheats. So far, the admins didn't notice.",
        "{0}: HELP MEEEEE, EIRIN!",
        "{0}: HELP MEEEEE, {1}!",
        "{0} has a [[SPECIL DEAL]] for you.",
        "{0} has a [[SPECIL DEAL]] for {1}.",
        "{0} did [Hyperlink Blocked] with {1}.",
        "{0} made [Hyperlink Blocked] with {1}.",
        "{0} [Hyperlink Blocked] {1}.",
        "{0} made an Item Shop! Avaliable items: {1} for only 5G, and {2} for only 9995G.",
        "{0} wants to reincarnate into a slime in their next life.",
        "{0} wants to reincarnate into a dog in their next life.",
        "{0} wants to reincarnate into a cat in their next life.",
        "{0} wants to reincarnate into a catgirl in their next life.",
        "{0} wants to reincarnate into a vending machine in their next life.",
        "{0} wants to reincarnate into a warrior in their next life.",
        "{0} wants to reincarnate into a hero in their next life.",
        "{0} wants to reincarnate into a spider in their next life.",
        "{0} wants to reincarnate into a facist in their next life.",
        "{0} wants to reincarnate into a monster in their next life.",
        "{0} wants to reincarnate into a [NSFW CONTENT] in their next life.",
        "{0} wants to reincarnate into a human in their next life.",
        "{0} wishes to be tiny.",
        "{0} wishes to be giant.",
        "{0} starts singing Rasputin, but about {1} instead.",
        "{0} utterly massacred {1}... in Super Smash Bros. Ultimate.",
        "{0} utterly massacred {1}... in Super Smash Bros. Brawl.",
        "{0} utterly massacred {1}... in Naruto Shippuden Ultimate Ninja Storm 4.",
        "{0} utterly massacred {1}... in Naruto Shippuden Ultimate Ninja Storm 3 Fullburst.",
        "{0} utterly massacred {1}... in Minecraft.",
        "{0} utterly massacred {1}... in bed.",
        "{0} utterly massacred {1}... in speedrunning.",
        "{0}: NONONO! I feel good.",
        "{0}: YESYESYES! I feel bad.",
        "{0} and {1} have a spicy discussion about the upcoming episode.",
        "{0} and {1} have a spicy discussion about if either aliens or ghosts are real.",
        "{0} and {1} have a spicy discussion about if either aliens or ghosts are real, while {2} unveils themselves in front of them both as an alien ghost.",
        "{0}: Hey hey hey. Listen to me, {1}.",
        "{0}: Well, there's definitely a gap here!\n\n\n",
        "{0} won the battle!\nNot really.",
        "{0} drank an Espresso Macchiato.",
        "Tell me one thing: why is {0} even in this game?!",
        "{0} has ice on their teeth.",
        "{0} has ice on their tee.",
        "{0} needs a Noise.",
        "{0} stepped into a mysterious portal and was transported into a strange alternate dimension where everyone spelled \"colour\" in a different way.",
        "{0} stepped into a mysterious portal and was transported into a strange alternate dimension where everyone spelled \"color\" in a different way.",
        "{0} stepped into a mysterious portal and was transported into a strange alternate dimension where everyone spelled \"biscuit\" in a different way.",
        "{0} stepped into a mysterious portal and was transported into a strange alternate dimension where everyone spelled \"cookie\" in a different way.",
        "{0} decided to level up solo.",
        "{0}: I have a pen, I have a apple... Ah! Apple pen!",
        "{0}: I have a pen, I have pineapple... Ah! Pineapple pen!",
        "{0}: Apple pen, pineapple pen... Ah! Pen Pineapple Apple Pen!",
        "{0}: I have a pen, I have a pen... Ah! Long pen!",
        "{0}: I have a apple, I have pineapple... Ah! Apple pineapple!",
        "{0}: Long pen, apple pineapple... Ah! Pen Pineapple Apple Pen!",
        "{0}: I have a {1}, I have a {2}... Ah! {1} {2}!",
        "{0} has a rave in {1}'s future grave.",
        "{0} has made a legal contract with {1}.",
        "{0} has made an illegal contract with {1}.",
        "{0}: How much is the fish?",
        "{0}: WHY BE THE [Little Sponge] WHO HATES ITS [$4.99] LIFE\n{0}: WHEN YOU CAN BE A\n{0}: [BIG SHOT!!!]",
        "{0}: HEY MASTER. I AM MAD SCIENTIST. SO COOOOL. SONUVABITCH!",
        "{0}: I'm...! The bad guy.",
        "{0} remembers their past victories (if any).",
        "{0} drank a buff potion that was literally labeled \"Buff Potion\", and immediately became swole.",
        "{0} used a calculator to determine their exact odds of winning, and didn't like what they saw. At least they didn't die in this scene.",
        "{0} finally understood the true meaning of friendship. People who stab you are not friends. People who don't stab you are friends. Probably.",
        "{0} took a potato chip... and ate it!",
        "{0} tried to deal a finishing blow to {1}, but missed due to a programming oversight that caused all attacks to have a 1/256 chance of missing.",
        "{0} had a weird dream, with lots and lots of walking.",
        "Kogasa Tatara surprises you!",
        "I bet you'll forget about this message, even if you noticed it.",
        "[this filler message was censored]",
        "No message."
    ];
string charactersFile = $@"{AppDomain.CurrentDomain.BaseDirectory}\characters.txt";
Console.WriteLine("Welcome to Epic Battles! Checking the presence of 'characters.txt'...");
logLines.Add("Players participating in the battle:");
if (!File.Exists(charactersFile))
{
    Console.WriteLine("Character file doesn't exist, entering manual input mode.\nProvide the number of players:");
    numberOfPlayers = int.Parse(SanitizeTextInput(Console.ReadLine()!, "5", true));
    List<string> players = new List<string>(numberOfPlayers);
    List<string> deadPlayers = new List<string>();
    for (int i = 0; i < numberOfPlayers; i++)
    {
        Console.WriteLine($"Provide the name of player {i + 1}: ");
        string sanitizedName = SanitizeTextInput(Console.ReadLine()!, $"Player {i + 1}", false);
        logLines.Add(sanitizedName);
        players.Add(sanitizedName);
    }
    logLines.Add("\nBattle start!\n");
    GameLoop(numberOfPlayers, players, deadPlayers);
}
else
{
    string[] fileContents = File.ReadAllLines(charactersFile);
    List<string> players = new List<string>();
    List<string> deadPlayers = new List<string>();
    int i = 0;
    foreach (string line in fileContents)
    {
        string sanitizedName = SanitizeTextInput(line, $"Player {i + 1}", false);
        logLines.Add(sanitizedName);
        players.Add(sanitizedName);
        i++;
    }

    if (players.Count == 0)
    {
        Console.WriteLine("Nothing found in the character file, entering manual input mode.");
        Console.WriteLine("Provide the number of players:");
        numberOfPlayers = int.Parse(SanitizeTextInput(Console.ReadLine()!, "5", true));
        for (i = 0; i < numberOfPlayers; i++)
        {
            Console.WriteLine($"Provide the name of player {i + 1}: ");
            string sanitizedName = SanitizeTextInput(Console.ReadLine()!, $"Player {i + 1}", false);
            logLines.Add(sanitizedName);
            players.Add(sanitizedName);
        }
    }
    else
    {
        Console.WriteLine("Characters found in the character file. Starting the battle automatically.");
        Thread.Sleep(1000);
    }
    logLines.Add("\nBattle start!\n");
    GameLoop(players.Count, players, deadPlayers);
}
void GameLoop(int numberOfPlayers, List<string> players, List<string> deadPlayers)
{
    Random random = new Random();
    int verdict;
    string message;
    Console.Clear();
    while (deadPlayers.Count < numberOfPlayers - 1)
    {
        do
        {
            victim = players[random.Next(0, players.Count)];
            killer = players[random.Next(0, players.Count)];
            if (players.Count > 2) bystander = players[random.Next(0, players.Count)];
            else if (victim != "Someone else" && killer != "Someone else") bystander = "Someone else";
            else bystander = String.Empty;
        } while (victim == killer || victim == bystander || killer == bystander);
        verdict = random.Next(0, 100);
        if (verdict < deathChance)
        {
            message = string.Format(deathMessages[random.Next(0, deathMessages.Length)], victim, killer, bystander) + $"{victim} died.";
            WriteMessage(message);
            players.Remove(victim);
            deadPlayers.Add(victim);
        }
        else
        {
            message = string.Format(fillerMessages[random.Next(0, fillerMessages.Length)], victim, killer, bystander);
            WriteMessage(message);
        }
        Thread.Sleep(3000);
    }
    WriteMessage("\nThe battle has ended! Here are the results:");
    WriteMessage(String.Concat("1st place: ", players.First()));
    deadPlayers.Reverse();

    int place;
    for (int i = 0; i < deadPlayers.Count; i++)
    {
        place = i + 2;
        if (place % 10 == 1 && place % 100 != 11) WriteMessage($"{place}st place: " + deadPlayers[i]);
        else if (place % 10 == 2 && place % 100 != 12) WriteMessage($"{place}nd place: " + deadPlayers[i]);
        else if (place % 10 == 3 && place % 100 != 13) WriteMessage($"{place}rd place: " + deadPlayers[i]);
        else WriteMessage($"{place}th place: " + deadPlayers[i]);
    }
    File.WriteAllLines(logFilePath, logLines, System.Text.Encoding.UTF8);
    Console.WriteLine($"\nBattle log saved to {logFileName}");
    Console.ReadKey();
}

