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

-> setup_scene("START") ->

OFF: Welcome. Shall we start?

+ [Start Game] OFF: Click to continue when the little arrow appears in the bottom right of the text box or to instantly show the text.\\nHave fun!
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

    Information that we are going to an old grave
    grave is by an old leader of the great Viking invasion
    we can ask some questions that might help us later, but we can also not care much
    
    -> in_front_of_grave
    

== in_front_of_grave

    -> setup_scene("FRONT_OF_GRAVE") ->
    
    -> initial_talk
    
    = initial_talk
    
        DOC: So, here we are.
        DOC: Look at this Stanley, all those old runes. Magnificient, aren't they?
        OFF: Dr. Greenwood not even glances at you, not really expecting an answer.
        
        * STANLEY: Of course, Sir.
            ~ look("DOC", "STANLEY")
            >>> SLEEP 0.75
            ~ look("DOC", "RIGHT")
        * [Remain silent]
        
        -
        DOC: *mutters to himself* How do we get into this grave?
        
        * STANLEY: I'll think of something, Sir.
            ~ look("DOC", "STANLEY")
            DOC: Sure you'll do.
            ~ look("DOC", "RIGHT")
        * [Look around]
        
        -
        -> dialog_end -> choices

    = talk_to_doc
    
        { cycle:
            - OFF: Dr. Greenwood still stares at the runes, supporting his chin with one hand. And touching some of the runes with the other hand.
            - DOC: Hm, maybe those runes will do the trick.
            - DOC: *moving his fingers across the runes in patterns, he mutters to himself* Maybe now something will happen..
        }
        
        + STANLEY: Sir?
        
            ~ look("DOC", "STANLEY")
        
            DOC: Yes, Stanley?
            
            -- (talk)
            ** (rune_knowledge) STANLEY: What do those runes say?
                DOC: It says: 'Here lies Torleif, fear of Saxons, may he feast well in Valhalla.'
            ** (boat_burial) STANLEY: I thought Vikings had ship burials?
                DOC: Yes, they had.
            ** {boat_burial} STANLEY: And then the boat is put to the sea and set to fire?
                DOC: Common misconception, Stanley. There were some sea burials, but most high ranking Vikings were simply buried in their boats.
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
        DOC: Aha! See Stanley? I knew I was into something with these runes.
        
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
            - DOC: Hm, maybe those runes will do the trick.
            - DOC: *moving his fingers across the runes in patterns, he mutters to himself* Maybe now something will happen..
        }
        
        + STANLEY: Sir?
        
            ~ look("DOC", "STANLEY")
        
            DOC: Yes, Stanley?
            
            -- (talk)
            ** (rune_knowledge2) STANLEY: What do those runes say?
                DOC: It says: 'Speak openly and pass in.'
            ** (runes_not_making_sense) {runesOnWallSeen} STANLEY: Have you seen those big runes on the wall?
                DOC: Of course, I have seen them. But they do not make any sense.
            ** {runes_not_making_sense} STANLEY: Why do those runes make no sense?
                DOC: They spell, 'Y N P L O E A'. No meaning in that.
                ~ runeNames = true
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
    
        STANLEY: I should help Mr. Greenwood.
        
        -> dialog_end -> choices

== at_the_burial_chamber

    -> setup_scene("BURIAL_CHAMBER") ->
    
    -> initial_talk
    
    = initial_talk
        
        DOC: Here we are. // TODO
        ~ look("DOC", "RIGHT")
        >>> SLEEP 0.5
        ~ look("DOC", "LEFT")
        >>> SLEEP 0.5
        ~ look("DOC", "RIGHT")
        >>> SLEEP 0.5
        ~ look("DOC", "LEFT")
        >>> SLEEP 0.5
        DOC: Astonishing!

    -> outside_grave
    

== outside_grave

    -> setup_scene("OUTSIDE_GRAVE") ->

    DOC: What an adventure, Stanley, and how I solved all those puzzles single-handedly. // TODO
    
    * STANLEY: Yes, Sir.
    * STANLEY: I quit, Sir.

    -
    -> the_end
    
== the_end

    -> setup_scene("THE_END") ->
    
    OFF: Thanks for playing! // TODO
    
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
    
            OFF: As you touch this rune, the door opens, yay. // TODO
            
            ~ walkToDoor()
            ~ event("DOOR_OPEN")
            ~ look("DOC", "RIGHT")
            
            DOC: Aha, I spoke openly about my feelings. That must have opened the door!
            
            -> at_the_burial_chamber
            
        OFF: Dr. Greenwood flinches shortly.
        
        - touchedRuneCount >= 4:
        
            OFF: As this rune begins to glow, all runes stop to glow one after another with this rune being the last. {runeNames:If you remember correct, you spelled, '{touchedRunes}'. Why isn't anything happening?}
            
            ~ touchedRunes = ""
            ~ touchedRuneCount = 0
    }
    
    ->->    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
