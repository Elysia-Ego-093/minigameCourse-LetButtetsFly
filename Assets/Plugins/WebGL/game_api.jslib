mergeInto(LibraryManager.library, {

    GetGameData: function(json)
    {
        if (window.GetGameData)
        {
            let data = UTF8ToString(json);
            window.GetGameData(data);
        }
        else
        {
            console.log("GetGameData不存在");
        }
    },

    GetPlayerUid: function()
    {
        if (window.GetPlayerUid)
        {
            return window.GetPlayerUid();
        }
        else
        {
            console.log("GetPlayerUid不存在");
        }
    },

    GetStatus: function(json)
    {
        if (window.GetStatus)
        {
            let data = UTF8ToString(json);
            window.GetStatus(data);
        }
        else
        {
            console.log("GetStatus不存在");
        }
    },

    InitialStatus: function(id, character_id)
    {
        if (window.InitialStatus)
        {
            window.InitialStatus(id, character_id);
        }
        else
        {
            console.log("InitialStatus不存在");
        }
    },

    GameStart: function()
    {
        if (window.GameStart)
        {
            return window.GameStart();
        }
        else
        {
            console.log("GameStart不存在");
        }
    },

    GameOver: function()
    {
        if (window.GameOver)
        {
            return window.GameOver();
        }
        else
        {
            console.log("GameOver不存在");
        }
    }

});