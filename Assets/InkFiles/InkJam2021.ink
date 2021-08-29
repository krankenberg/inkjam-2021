EXTERNAL setupScene(sceneName)
EXTERNAL endDialog()

-> briefing

== function setupScene(sceneName) ==

    // Scene {sceneName} will be set up.
    
    ~ return

== function endDialog() ==

    // Free play
    
    ~ return

== briefing

    Information that we are going to an old graveyard
    Graveyard is by an old leader of the great Viking invasion
    we can ask some questions that might help us later, but we can also not care much
    
    -> in_front_of_graveyard
    

== in_front_of_graveyard

    -> setup_scene("FRONT_OF_GRAVEYARD") ->
    
    -> initial_talk
    
    = initial_talk
    
        DOC: So, here we are.
        DOC: Look at this Stanley, all those old runes. Magnificient, aren't they?
        OFF: Dr. Greenwood not even glances at you, not really expecting an answer.
        
        * STANLEY: Of course, Sir.
        * [Remain silent]
        
        -
        DOC: *mutters to himself* How do we get into this graveyard?
        
        * STANLEY: I'll think of something, Sir.
            DOC: Sure you'll do.
        * [Look around]
        
        -
        -> dialog_end -> choices

    = talk_to_doc
    
        OFF: Dr. Greenwood still stares at the runes, supporting his chin with one hand. And touching some of the runes with the other hand.
        
        + STANLEY: Sir?
        
            DOC: Yes, Stanley?
            
            -- (talk)
            ** STANLEY: What do those runes say?
                DOC: It says: 'Here lies Torleif, fear of Saxons, may he feast well in Valhalla.'
            ** (boat_burial) STANLEY: I thought Vikings had ship burials?
                DOC: Yes, they had.
            ** {boat_burial} STANLEY: And then the boat is put to the sea and set to fire?
                DOC: Common misconception, Stanley. There were some sea burials, but most high ranking Vikings were simply buried in their boats.
            ++ [Go away]
                STANLEY: Thanks, Sir.
                -> dialog_end -> choices
            
            -- -> talk
                
        + [Go away]
            -> dialog_end -> choices
            
    = look_at_entrance
    
        OFF: A huge rock is blocking the entrance.
        
        * [Push the rock aside]
            OFF: You simply **push** the rock aside.
            -> entrance_opened
        + [Go away]
            -> dialog_end -> choices
        
    = choices
    
        Choices
    
        + [Talk to Dr. Greenwood]
            -> talk_to_doc
        + [Look at the entrance]
            -> look_at_entrance
            
    = entrance_opened
    
        OFF: Dr. Greenwood flinches shortly.
        DOC: Aha! See Stanley? I knew I was into something with these runes.

        -> at_the_burial_chamber
    

== at_the_burial_chamber

    -> setup_scene("BURIAL_CHAMBER") ->
    
    -> initial_talk
    
    = initial_talk
        
        DOC: Here we are.

    -> outside_graveyard
    

== outside_graveyard

    -> setup_scene("OUTSIDE_GRAVEYARD") ->

    DOC: What an adventure, Stanley, and how I solved all those puzzles single-handedly.
    
    * STANLEY: Yes, Sir.
    * STANLEY: I quit, Sir.

    -
    -> END
    

== dialog_end

    ~ endDialog()
    
    ->->
    
    
== setup_scene(sceneName)

    ~ setupScene(sceneName)
    
    Setup Scene {sceneName}. # JUST_INKY
    
    ->->
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    