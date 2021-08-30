EXTERNAL setupScene(sceneName)
EXTERNAL endDialog()
EXTERNAL sleep(time)
EXTERNAL look(who, where)
EXTERNAL event(eventName)
EXTERNAL walkToDoc()
EXTERNAL walkToDoor()

VAR touchedRunes = ""
VAR touchedRuneCount = 0
VAR runesOnWallSeen = false
VAR runeNames = false
VAR sawTorch = false
VAR runeTryCounter = 0
VAR triedOpen = false
VAR opna = false
VAR quit = false

-> setup_scene("START") ->

OFF: Welcome. Shall we start?

+ [Start Game] OFF: Click to continue when the little arrow appears in the bottom right of the text box or to instantly show the text.\\nHave fun!
    STANLEY: Dr. Greenwood rang the bell to signal that I should seek him out.
    
    ** STANLEY: As a good butler, I obeyed the command immediately.[] Normally, he should be in the saloon at this time.
    ** (finish_tea_first) STANLEY: I finished my tea first.[] After that, I sought him out. Normally, he is in the saloon at this time.
    
    -
    OFF: You enter the saloon.
    -> briefing

== function setupScene(sceneName) ==

    // Scene {sceneName} will be set up.
    
    ~ return

== function endDialog() ==

    // Free play
    
    ~ return

== function sleep(time) ==

    // sleep
    
    ~ return
    
== function look(who, where) ==

    // look

    ~ return
    
== function event(eventName) ==

    // event

    ~ return
    
== function walkToDoc() ==

    // event

    ~ return
    
== function walkToDoor() ==

    // event

    ~ return

== briefing

    -> setup_scene("BRIEFING") ->
    
    STANLEY: Sir?
    ~ look("DOC", "STANLEY")
    
    DOC: Stanley, {finish_tea_first: where have you been that long?| fast as always!} Listen: a colleague of mine brought a new occasion to my attention. You need to pack at once.
    
    - (accompany_or_not)
    * (question_accompany) STANLEY: Shall I accompany you, Sir?
        DOC: Naturally, Stanley. What would a gentleman be without his butler?
        -> accompany_or_not
    * STANLEY: Where are we going, Sir?
        -> location
    
    - (location)
    DOC: We are going north, so pack for cold days. Our destination is an old grave.
    
    - (infos)
    * (ask_for_grave) STANLEY: What kind of grave is that?
        DOC: We are going to the grave of an old Viking leader. He died during the Great Viking invasion. My colleague told me that there should be a precious artifact.
        -> infos
    * {ask_for_grave} STANLEY: How come, nobody else entered that grave before, Sir?
        DOC: Aha, because of course no one is as clever a historian as I am, Stanley.
        -> infos
    * [STANLEY: I will pack at once, Sir.]
        ~ look("STANLEY", "RIGHT")
        STANLEY: I will pack at once, Sir.
    
    -
    DOC: Godspeed, Stanley!
    ~ look("DOC", "LEFT")
    {
        - finish_tea_first && question_accompany:
            DOC: What is up with this Stanley, lately? Getting more and more cheeky.
        - finish_tea_first || question_accompany:
            DOC: Ah, my good Stanley. A little bit cheeky form time to time, but still.
        - else:
            DOC: Ah, my good Stanley. Always dutiful.
    }
    
    -> in_front_of_grave
    

== in_front_of_grave

    -> setup_scene("FRONT_OF_GRAVE") ->
    
    -> initial_talk
    
    = initial_talk
    
        DOC: So, here we are.
        DOC: Look at this Stanley, all those old runes. Magnificient, aren't they?
        OFF: Dr. Greenwood not even glances at you, not really expecting an answer.
        
        * STANLEY: Indeed, Sir.
            ~ look("DOC", "STANLEY")
            >>> SLEEP 0.75
            ~ look("DOC", "RIGHT")
        * [Remain silent]
        
        -
        DOC: *mutters to himself* How do we get into this grave?
        
        * STANLEY: I'll think of something, Sir.
            ~ look("DOC", "STANLEY")
            >>> SLEEP 0.5
            ~ look("DOC", "RIGHT")
            DOC: Yes yes, sure you'll do.
        * [Look around]
        
        -
        -> dialog_end -> choices

    = talk_to_doc
    
        { cycle:
            - OFF: Dr. Greenwood still stares at the runes, supporting his chin with one hand. And touching some of the runes with the other hand.
            - DOC: Hm, maybe those runes will do the trick.
            - DOC: *moving his fingers across the runes in patterns, he mutters to himself* Maybe now something will happen..
        }
        
        + STANLEY: {Sir\?|Sir\?|Sorry to interrupt you again, Sir.}
        
            ~ look("DOC", "STANLEY")
        
            DOC: {Yes, Stanley?|Stanley?|*Dr. Greenwood just turns around to you without saying anything.*}
            
            -- (talk)
            ** (rune_knowledge) STANLEY: What do those runes say?
                DOC: It says: 'Here lies Torleif, fear of Saxons, may he feast well in Valhalla.'
            ** (boat_burial) STANLEY: I thought Vikings had ship burials?
                DOC: Yes, they had.
            ** {boat_burial} STANLEY: And then the boat is put to the sea and set to fire?
                DOC: Common misconception, Stanley. There were some sea burials, but most high ranking Vikings were simply buried in their boats.
            ** (asked_about_stones){look_at_stones} STANLEY: Have you already seen those strange stones, Sir?
                DOC: I really don't care about some stones, Stanley.
            ++ [Go away]
                STANLEY: Thanks, Sir.
                ~ look("DOC", "RIGHT")
                -> dialog_end -> choices
            
            -- -> talk
                
        + [Go away]
            -> dialog_end -> choices
            
    = look_at_entrance
    
        OFF: A huge rock is {|still} blocking the entrance. {||Maybe there is some sort of mechanism to open it.}
        
            -> dialog_end -> choices
            
    = look_at_runestone
        
        OFF: You look at a huge stone with small rune letters engraved in it. {rune_knowledge: Dr. Greenwood said, that they say, 'Here lies Torleif, fear of Saxons, may he feast well in Valhalla.'|They don't make any sense to you.}
        
        -> dialog_end -> choices
            
    = look_at_stones
    
        ~ event("STONE_PUZZLE")
        OFF: You look at some stones. {|They seem to be placed wrongly.} {||Maybe they can be rotated.|Maybe they can be rotated to form some sort of shape.}
        
        -> dialog_end -> choices
        
    = choices
    
        Choices
    
        + [Talk to Dr. Greenwood]
            -> talk_to_doc
        + [Look at the entrance]
            -> look_at_entrance
        + [Look at the runestone]
            -> look_at_runestone
        + [Look at the stones]
            -> look_at_stones
        + [solve stones]
            -> entrance_opened
            
    = entrance_opened
    
        OFF: The earth shakes, something is happening...
        ~ walkToDoc()
        ~ event("ENTRANCE_OPEN")
        ~ look("DOC", "LEFT")
        OFF: Dr. Greenwood flinches shortly.
        DOC: Aha! See Stanley? I knew I was into something with these runes. {asked_about_stones: Have you finished playing with your stones there? Great things are going to happen!|}
        
        * STANLEY: Of course, Sir.
        * [Remain silent]

        -
        OFF: Dr. Greenwood rushes into the grave. You follow shortly after.
        -> room_1
    

== room_1

    -> setup_scene("ROOM_1") ->

    OFF: Dr. Greenwood already stands in front of another runestone and examines it.
    
    -> dialog_end -> choices
            
    = choices
    
        Choices
    
        + [Talk to Dr. Greenwood]
            -> talk_to_doc
        + [Look at the entrance]
            -> look_at_entrance
        + [Look at the runestone]
            -> look_at_runestone
        + [Touch O]
            -> press_runic_o
        + [Touch P]
            -> press_runic_p
        + [Touch E]
            -> press_runic_e
        + [Touch N]
            -> press_runic_n
        + [Touch L]
            -> press_runic_l
        + [Touch Y]
            -> press_runic_y
        + [Touch A]
            -> press_runic_a
        + [Torch]
            -> torch
        + [Way out]
            -> way_back
            
    = press_runic_y
    
        -> touch_rune("Y") -> dialog_end -> choices
            
    = press_runic_a
    
        -> touch_rune("A") -> dialog_end -> choices
            
    = press_runic_l
    
        -> touch_rune("L") -> dialog_end -> choices
            
    = press_runic_n
    
        -> touch_rune("N") -> dialog_end -> choices
            
    = press_runic_e
    
        -> touch_rune("E") -> dialog_end -> choices
            
    = press_runic_p
    
        -> touch_rune("P") -> dialog_end -> choices
            
    = press_runic_o
    
        -> touch_rune("O") -> dialog_end -> choices
            
    = talk_to_doc // TODO

        { cycle:
            - OFF: Dr. Greenwood still stares at the runes, supporting his chin with one hand. And touching some of the runes with the other hand.
            - DOC: Hm, maybe **those** runes will do the trick like outdoors.
            - DOC: *moving his fingers across the runes in patterns, he mutters to himself* Maybe now something will happen..
            - DOC: {opna:Opna! .. Opna?|Speak openly...}
        }
        
        + STANLEY: {Sir\?|Sir\?|Sorry to interrupt you again, Sir.}
        
            ~ look("DOC", "STANLEY")
        
            DOC: {Yes, Stanley?|Stanley?|*Dr. Greenwood just turns around to you without saying anything.*}
            
            -- (talk)
            ** (rune_knowledge2) STANLEY: What do those runes say?
                DOC: It says: 'Speak openly and pass in.'
            ** (runes_not_making_sense) {runesOnWallSeen} STANLEY: Have you seen those big runes on the wall?
                DOC: Of course, I have seen them. But they do not make any sense.
            ** {runes_not_making_sense} STANLEY: Why do those runes make no sense?
                DOC: They spell, 'Y N P L O E A'. No meaning in that.
                ~ runeNames = true
            ** {rune_knowledge2} STANLEY: Are you certain it says, 'openly'?
                DOC: Needless to say, I am certain.
            ** {triedOpen || runeTryCounter > 3} STANLEY: What is 'open' in old norse?
                ~ opna = true
                DOC: 'OPNA', aha, I just came to an idea.
                ~ look("DOC", "RIGHT")
                DOC: *loud* OPNA!
                >>> SLEEP 0.75
                OFF: Nothing happens.
                -> dialog_end -> choices
            ** {sawTorch} STANLEY: Have you lit all those torches?
                DOC: No, Stanley. Have you?
                
                *** STANLEY: Yes, Sir.
                    DOC: Fast as always with your tasks, Stanley.
                *** STANLEY: No, Sir.
                    DOC: Wouldn't that have been your natural task?
                    STANLEY: So who lit them? Or how did they stay on that long?
                    DOC: We have more pressing business here, Stanley...
                    
                    **** STANLEY: But[ isn't it strange that they are lit?]...
                        ~ look("DOC", "RIGHT")
                        OFF: Dr. Greenwood raises an eyebrow and then turns away from you.
                        -> dialog_end -> choices
                         
                    **** [Remain silent]
                
            ++ [Go away]
                STANLEY: Thanks, Sir.
                ~ look("DOC", "RIGHT")
                -> dialog_end -> choices
            
            -- -> talk
                
        + [Go away]
            -> dialog_end -> choices
        
    = look_at_runestone
        
        OFF: You look at a huge stone with small rune letters engraved in it. {rune_knowledge2: Dr. Greenwood said, that they say, 'Speak openly and pass in.'|They don't make any sense to you.}
        
        -> dialog_end -> choices
            
    = look_at_entrance
    
        OFF: A big door  is {|still} blocking the entrance. {||Maybe there is some sort of mechanism to open it.}
        
        -> dialog_end -> choices
        
    = torch
    
        ~ sawTorch = true

        { shuffle:
            - OFF: It's a torch.
            - STANLEY: *to himself* How are those torches burning that long?
            - OFF: A burning torch.
        }
        
        -> dialog_end -> choices
        
    = way_back
    
        STANLEY: I should help Dr. Greenwood.
        
        -> dialog_end -> choices

== at_the_burial_chamber

    -> setup_scene("BURIAL_CHAMBER") ->
    
    -> initial_talk
    
    = initial_talk
        
        DOC: Look at this, Stanley.
        ~ look("DOC", "RIGHT")
        >>> SLEEP 0.5
        ~ look("DOC", "LEFT")
        >>> SLEEP 0.5
        ~ look("DOC", "RIGHT")
        >>> SLEEP 0.5
        ~ look("DOC", "LEFT")
        >>> SLEEP 0.5
        DOC: Astonishing! And there in the boat, I see the artifact. Stanley..
        STANLEY: Yes, Sir.
        OFF: You climb into the boat and get the artifact, as soon as you climb out again Dr. Greenwood takes it and looks at it.
        DOC: Fascinating, now let's get out of here. It's getting cold.
        
    -> outside_grave
    

== outside_grave

    -> setup_scene("OUTSIDE_GRAVE") ->

    DOC: What an adventure, Stanley, and how I solved all those puzzles single-handedly.
    
    * STANLEY: Another great achievement, Sir.
    * STANLEY: I quit, Sir.[] It has been enough. I am doing all the hard work. And you are taking credit for it.
        ~ quit = true
        DOC: Outrageous! How could you...
        ~ look("STANLEY", "LEFT")
        OFF: As you wander off, you can't hear what he is saying.

    -
    -> the_end
    
== the_end

    -> setup_scene("THE_END") ->
    
    {
        - quit: OFF: Stanley began to work for another Lord, who honours his work and even mentions his name in his publications.
        - else: OFF: Always thinking of something, Stanley helps Dr. Greenwood until this day. Never mentioned by him in any publication.
    }
    
    OFF: Thanks for playing!
    
    -
    -> END
    

== dialog_end

    ~ endDialog()
    
    ->->
    
    
== setup_scene(sceneName)

    ~ setupScene(sceneName)
    
    Setup Scene {sceneName}. # JUST_INKY
    
    ->->
    
== touch_rune(rune)

    ~ runesOnWallSeen = true

    { 
        - touchedRunes ? rune: 
    
            OFF: You touch the already glowing rune, all runes stop to glow at once.
            
            ~ touchedRunes = ""
            ~ touchedRuneCount = 0
    
        - else:
            
            ~ touchedRunes += rune
    
            OFF: You touch the rune{runeNames: standing for '{rune}'|}. It begins to glow.

            ~ touchedRuneCount += 1
    }
    
    { 
        - touchedRuneCount == 4 && touchedRunes == "OPNA":
    
            OFF: As you touch this rune, suddenly the earth shakes again, and you hear rumbling from the door.
            
            ~ walkToDoor()
            ~ event("DOOR_OPEN")
            ~ look("DOC", "RIGHT")
            
            DOC: {opna:Aha, I said 'OPNA' in the right intonation or maybe now it acknowledged me openly talking about my feelings.|Aha, I spoke openly about my feelings.} That must have opened the door!
            
            -> at_the_burial_chamber
        
        - touchedRuneCount >= 4:
        
            OFF: As this rune begins to glow, all runes stop to glow one after another with this rune being the last. {runeNames:If you remember correct, you spelled, '{touchedRunes}'. Why isn't anything happening?} {opna: Maybe I should try 'OPNA'.|}
            
            {touchedRunes == "OPEN":
                ~ triedOpen = true
            }
            
            ~ runeTryCounter += 1
            ~ touchedRunes = ""
            ~ touchedRuneCount = 0
    }
    
    ->->    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    