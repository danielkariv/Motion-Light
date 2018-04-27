
// show the given page, hide the rest
function show(elementID) {
    // try to find the requested page and alert if it's not found
    var ele = document.getElementById(elementID);
    if (!ele) {
        alert("no such element");
        return;
    }

    // get all pages, loop through them and hide them
    var pages = document.getElementsByClassName('page');
    for (var i = 0; i < pages.length; i++) {
        pages[i].style.display = 'none';
    }
    // then show the requested page
    ele.style.display = 'block';
}
function SignupUser(username, password, email, country) {
    // check input
    var isInputRight = false;
    isInputRight = (username != "" && username.length >= 6 && password != "" && username.length >= 6 && email != "" && country)
    if (isInputRight) {
        var data = {
            username: username,
            password: password,
            email: email,
            countryName: country
        };
        // ajax call
        data = JSON.stringify(data);
        $.ajax({
            type: "POST",
            url: "WebService1.asmx/RegisterUser",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == 1) {
                    // success to sign up
                    console.log("Successed to sign up!")
                    GetUserInfo(username, password);
                    // move forward to after function
                }
                else {
                    // fail to sign up, username may be already used
                    console.log("Failed to sign up!")
                }
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    } else {
        // input wrong, let user know
    }
}
function LoginUser(username, password) {
    // check input
    var isInputRight = false;
    isInputRight = (username != "" && username.length >= 6 && password != "" && username.length >= 6)
    if (isInputRight) {
        var data = {
            username: username,
            password: password
        };
        data = JSON.stringify(data);
        // ajax call
        $.ajax({
            type: "POST",
            url: "WebService1.asmx/LoginUser",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == 1) {
                    // success to login up
                    console.log("Successed to login up!");
                    GetUserInfo(username, password);
                    
                    // move forward to after function
                    location.href = "#";
                    show('Home');
                }
                else {
                    // fail to sign up, may not exist or wrong password
                    console.log("Failed to login up!");
                }
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    } else {
        // input wrong, let user know
    }
}
function GetCountries() {
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/GetCountries",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            HTMLCountries(result.d);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
function HTMLCountries(countries) {
    // get list of countries names, make html option and place it inside all html element with  .countryList class
    for (var i = 0; i < countries.length; i++) {
        var name = countries[i];
        var html;
        html = "<option value='" + name + "'>" + name + "</option>";
        $(".countryList").append(html);
    }
}
function GetUserInfo(username, password) {
    // check input
    if (username != null && password != null) {
        if(username.length >= 6 && password.length >= 6){
        var data = {
            username: username,
            password: password
        };
        data = JSON.stringify(data);
        $.ajax({
            type: "POST",
            url: "WebService1.asmx/GetUserInfo",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d) {
                    console.log("Sucessed to get user info! \n " + result.d);
                    var obj = JSON.parse(result.d);
                    HTMLUpdateUserInfo(obj.id, obj.username, obj.password, obj.email, obj.countryName);
                    $(".usermode").show();
                    $(".guestmode").hide();
                } else {
                    LogOut();
                }
            },
            failure: function (response) {
                alert(response.d);

            }
        });
        }
        else {
            LogOut();
        }
    }else{
        LogOut();
    }
    
}
function HTMLUpdateUserInfo(userid,username,password,email,countryName){
    sessionStorage.setItem('userid', userid);
    sessionStorage.setItem('user', username);
    sessionStorage.setItem('pass', password);
    sessionStorage.setItem('email', email);
    sessionStorage.setItem('countryName', countryName);
    
}
function GetServers() {
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/GetAllServers",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("Got the server list!");
            HTMLServers(result.d);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
function HTMLServers(servers) {
    $(".serverlist").empty();
    if (servers) {
        for (var i = 0; i < servers.length; i++) {
            try {
                //console.log(servers[i]);
                var obj = JSON.parse(servers[i]);
                var id = obj.ID;
                var name = obj.Name;
                var hoster = obj.HostUsername;
                var offerData = obj.OfferData[0];
                var html;
                // need to change when chaning design
                html = "<tr><th>" + name + " <br> " + hoster + " </th> <th><button onclick='P2P.ConnectTo(" + JSON.stringify(offerData) + "," + id + ");'>Join</button></th></tr>";
                $(".serverlist").append(html);
            } catch (err) {
                console.log(err);
            }
        }
    } else {
        html = "<tr><th> No server are running right now </th><th></th></tr>";
        $(".serverlist").append(html);
    }
}
var LFA;
function CreateServer(name, username, offerData) {
    if (name != "" && username && offerData) {
        var data =
        {
            name: name,
            hostUsername: username,
            offerData: JSON.stringify(offerData)
        }
        $.ajax({
            type: "POST",
            url: "WebService1.asmx/InsertServer",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d == -1) {
                    console.log("Failed to create server");
                } else {
                    P2P.room_id = result.d;
                    console.log("success to create server! moving to lobby room");
                    //document.getElementById('messages').textContent += "Waiting for player...\n";
                    location.href = '#Lobby';
                    show('Lobby');
                    $(".answers_model").fadeIn(0);
                    GetAnswers(P2P.room_id);
                    LFA = setInterval(function () {
                        GetAnswers(P2P.room_id);
                    }, 1000);
                }
                
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    } else {
        console.log("CreateServer: Wrong info")
    }
}

function RemoveServers(username){
    // make ajax
    // done! -> call after-function
}
var answer_id;
function SendAnswer(username, answerData) {
    if (username && answerData && P2P.room_id) {
        var answer =
        {
            sender: sessionStorage.getItem('user'),
            room_id: P2P.room_id,
            answerData: JSON.stringify(answerData)
        }
        $.ajax({
            type: "POST",
            url: "WebService1.asmx/InsertAnswer",
            data: JSON.stringify(answer),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.d != -1) {
                    answer_id = result.d;
                    console.log("Secess to send answer to the DB");
                } else {
                    console.log("Failed to send answer");
                }
                
                // when waiting to accpetion of the host, there should be a check if the server is still waiting for offer. if not, remove answer and go back to server list.
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }
    else {
        console.log("SendAnswer: Wrong info");
    }
}
function GetAnswers(roomid) {
    var data = {
        room_id: roomid
    }
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/GetAnswers",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d.length > 0)
                HTMLAnswers(result.d);
            else
                HTMLAnswers(null);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
function HTMLAnswers(answers) {
    $(".answerlist").empty();
    if (answers) {
        for (var i = 0; i < answers.length; i++) {
            try {
                //console.log(servers[i]);
                var obj = JSON.parse(answers[i]);
                console.log(obj);
                var id = obj.Id;
                var Sender = obj.Sender;
                var room_id = obj.Room_Id;
                var answerData = obj.AnswerData[0];
                var html;
                // need to change when chaning design
                html = "<tr><th style='width: 70%'><h3>" + Sender + "</h3></th><th style='width: 15%'><button onclick='AcceptAnswer(" + JSON.stringify(answerData) + "," + id + ");'>Accept</button></th><th style='width: 15%'><button onclick='DenieAnswer("+id+");'>Denie</button></th></tr>";
                $(".answerlist").append(html);
            } catch (err) {
                console.log(err);
            }
        }
    } else {
        html = "<tr><th> No answers </th><th></th><th></th></tr>";
        $(".answerlist").append(html);
    }
    // display it on list with button to accept and button to denie.
}
function AcceptAnswer(answerData,id) {
    P2P.peer.signal(answerData);
    RemoveAnswer(id);
}
function DenieAnswer(id) {
    RemoveAnswer(id);
}
function RemoveAnswer(id) {
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/DeleteAnswer",
        data: '{id: "' + id + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
function LogOut() {
    sessionStorage.clear();
    $(".usermode").hide();
    $(".guestmode").show();
}
/*
-----------------------
have 9 function ( may not be all) to make again!
better write the P2P script ( and add it here, as one file)
and write full game in JS and PHASER.IO ( somebody once told the world is gonna roll me..)
-----------------------
*/

// P2P stuff 
var P2P = {
    isloaded: false,
    peer: null,
    peer_data: null,
    room_id: null,
    ConnectTo: function (offerData, room_id) {
        P2P.InitPeer(false);
        //$(".se-pre-con").fadeIn("slow");
        $(".waiting_model").fadeIn("fast");
        console.log("TRYING CONNECT TO USING OFFERDATA: " + JSON.stringify(offerData));
        P2P.room_id = room_id;
        P2P.peer.signal(offerData);
    },
    InitPeer: function (ishost){
        P2P.isloaded = false;
        P2P.peer = null;
        P2P.peer_data = null;
        P2P.room_id = null;
		var cfg = {
			'iceServers': [{ url: 'stun:stun.l.google.com:19302' },
			{ url: 'stun:stun1.l.google.com:19302' },
			{ url: 'stun:stun2.l.google.com:19302' },
			{ url: 'stun:stun3.l.google.com:19302' },
			{ url: 'stun:stun4.l.google.com:19302' }]
		},
            con = { 'optional': [{ 'DtlsSrtpKeyAgreement': true }] }

        if (SimplePeer.WEBRTC_SUPPORT) {
            console.log("Init peer now! ishost: " + ishost);
            P2P.peer = new SimplePeer({
                initiator: ishost,
                trickle: false,
                config: cfg
            });
            P2P.peer.on('error', function (err) { console.log('error', err) });
            P2P.peer.on('signal', function (data) {
                if (P2P.peer.initiator) { // when host
                    P2P.peer_data = data;
                    P2P.isloaded = true;
                    // get answers and choose which one to take
                    
                } else { // when join ( aka when getting offer) it create answer
                    console.log("Created Answer: " + data);
                    P2P.peer_data = data;
                    P2P.isloaded = true;
                    SendAnswer(sessionStorage.getItem('user'), data);
                    $(".se-pre-con").fadeOut("slow");
                }
            });

            //when want to send data -> P2P.peer.send(YOUR_DATA);

            P2P.peer.on('data', function (data) {
                // when getting data
                var JSONdata = JSON.parse(data);
                if (JSONdata.action == "Dead") {
                    GL.GameStatus = "Done";
                    console.log("Game is done! " + JSONdata.player + " have died!");
                } else if (JSONdata.action == "Move") {
                    var player;
                    if (JSONdata.player == "Player1") {
                        player = GL.player1;
                    } else {
                        player = GL.player2;
                    }
                    var oldGridX = player.gridX; oldGridY = player.gridY;
                    player.gridX = JSONdata.x;
                    player.gridY = JSONdata.y;
                    if (player.playerID == 1)
                        GL.grid[oldGridY][oldGridX] = 4;
                    if (player.playerID == 2)
                        GL.grid[oldGridY][oldGridX] = 5;
                    //place light player behind
                    var dir;
                    var deltaX = player.gridX - oldGridX;
                    var deltaY = player.gridY - oldGridY;
                    if (deltaX == -1) {
                        dir = "left";
                    } else if (deltaX == 1) {
                        dir = "right"
                    } else if (deltaY == -1) {
                        dir = "up";
                    } else if (deltaY == 1) {
                        dir = "down";
                    }
                    player.dir = dir;
                    GL.PlaceLight(player, oldGridX, oldGridY);
                    GL.grid[player.gridY][player.gridX] = player.playerID;
                   
                    GL.UpdateSprite(player);
                }
            });
            P2P.peer.on('connect', function () {
                // when there is connection bettwen two clients
                console.log('Peer: CONNECT');
                
                if (location.href !== "#Lobby") {
                    location.href = "#Lobby";
                    show("Lobby");
                }
                $(".waiting_model").fadeOut("slow");
                $(".answers_model").fadeOut("slow");
                if (P2P.peer.initiator == true){
                    removeServer(sessionStorage.getItem('user'));
                    GL.WhoYou = "Player1";
                }
                else
                GL.WhoYou = "Player2";
                GL.GameStatus = "InGame";
                GL.UpdateInterID = setInterval(function () {
                    console.log("Tick!");
                    if (GL.GameStatus == "InGame") {
                        // host movement!
                        if (GL.WhoYou == "Player1")
                            GL.MoveTo(GL.player1, GL.player1.dir);
                        else
                            GL.MoveTo(GL.player2, GL.player2.dir)

                    }
                }, 500);
            });
            P2P.peer.on('close', function () {
                // when the connection closed
                console.log('Peer: DISCONNECT');
                location.reload();
            });
        } else {
            console.log("WebRTC NOT SUPPORTED");
        }
    }
}

// GAME stuff

var game;
function InitGame() {
    // Initialize Phaser, and create a 800px by 600px game
    game = new Phaser.Game(960, 600, Phaser.AUTO, 'game-area');

    // 0 = clear, 1 = player1, 2 = player2, 3 = wall, 4 = player1-light, 5 = player2-light
    // Create our 'main' state that will contain the game
    var mainState = {
        preload: function () {
            game.load.image('map', 'assets/map.png');
            game.load.image('player1', 'assets/player1.png');
            game.load.image('player2', 'assets/player2.png');
            game.load.image('player1_light', 'assets/player1_light.png');
            game.load.image('player2_light', 'assets/player2_light.png');
        },

        create: function () {
            // This function is called after the preload function     
            // Here we set up the game, display sprites, etc.  
            this.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL;
            this.scale.minWidth = 160;
            this.scale.minHeight = 100;
            this.scale.maxWidth = 1920;
            this.scale.maxHeight = 1200;

            cursors = game.input.keyboard.createCursorKeys();

            game.add.sprite(0, 0, 'map');
            GL.player1.sprite = game.add.sprite(GL.player1.gridX * 60, GL.player1.gridY * 60, 'player1');
            GL.grid[GL.player1.gridY][GL.player1.gridX] = 1;
            GL.player2.sprite = game.add.sprite(GL.player2.gridX * 60, GL.player2.gridY * 60, 'player2');
            GL.grid[GL.player2.gridY][GL.player2.gridX] = 2;
            GL.player1.sprite.scale.setTo(0.5, 0.5);
            GL.player1.sprite.anchor.setTo(0.5, 0.5);
            GL.player1.sprite.angle = 180;
            GL.player2.sprite.scale.setTo(0.5, 0.5);
            GL.player2.sprite.anchor.setTo(0.5, 0.5);
        },

        update: function () {
            // This function is called 60 times per second    
            // It contains the game's logic  
            if (GL.WhoYou != null)
                if (GL.WhoYou == "Player1") {
                if (GL.GameStatus == "InGame") {
                    if (cursors.left.isDown) {
                        GL.player1.dir = 'left';
                    }
                    if (cursors.right.isDown) {
                        GL.player1.dir = 'right';
                    }
                    if (cursors.up.isDown) {
                        GL.player1.dir = 'up';
                    }
                    if (cursors.down.isDown) {
                        GL.player1.dir = 'down';
                    }
                }
            } else if(GL.WhoYou == "Player2") {
                if (GL.GameStatus == "InGame") {
                    if (cursors.left.isDown) {
                        GL.player2.dir = 'left';
                    }
                    if (cursors.right.isDown) {
                        GL.player2.dir = 'right';
                    }
                    if (cursors.up.isDown) {
                        GL.player2.dir = 'up';
                    }
                    if (cursors.down.isDown) {
                        GL.player2.dir = 'down';
                    }
                }
            }
        },
    };

    // Add the 'mainState' and call it 'main'
    game.state.add('main', mainState);
    // Start the state to actually start the game
    game.state.start('main');
}
var GL = {
    WhoYou: null,
    // GameStatus = Waiting, InGame, Finish
    GameStatus: "Waiting",
    grid: [
            [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3],
            [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]
    ],
    GridMaxY: 10,
    GridMaxX: 16,
    player1: {
        playerID: 1,
        sprite: null,
        gridY: 2,
        gridX: 3,
        dir: 'right'
    },
    player2: {
        playerID: 2,
        sprite: null,
        gridY: 8,
        gridX: 13,
        dir: 'left'
    },
    MoveTo: function (player, dir) {
        var oldGridX = player.gridX, oldGridY = player.gridY;
        switch (dir) {
            case 'right':
                if (player.dir != 'left') {
                    player.gridX++;
                    player.dir = 'right';

                } else {
                    player.gridX--;
                };
                break;
            case 'left':
                if (player.dir != 'right') {
                    player.gridX--;
                    player.dir = 'left';

                } else {
                    player.gridX++;
                };
                break;
            case 'down':
                if (player.dir != 'up') {
                    player.gridY++;
                    player.dir = 'down';


                } else {
                    player.gridY--;
                };
                break;
            case 'up':
                if (player.dir != 'down') {
                    player.gridY--;
                    player.dir = 'up';

                } else {
                    player.gridY++;
                };
                break;
            default:
                if (player.dir == 'right')
                    player.gridX++;
                if (player.dir == 'left')
                    player.gridX--;
                if (player.dir == 'up')
                    player.gridY++;
                if (player.dir == 'down')
                    player.gridY--;
                break;
        }
        if (player.gridX < 0 || player.gridX > GL.GridMaxX || player.gridY < 0 || player.gridY > GL.GridMaxY) {
            // player is dead ( he is outside of the map)
            player.gridY = oldGridY;
            player.gridX = oldGridX;
            var data = {
                player: GL.WhoYou,
                action: "Dead"
            }
            GL.GameStatus = "Done";
            P2P.peer.send(JSON.stringify(data));
        }
        else {
            var pointId = GL.grid[player.gridY][player.gridX];
            if (pointId != 0) {
                //player is dead( collide with something)
                player.gridY = oldGridY;
                player.gridX = oldGridX;
                var data = {
                    player: GL.WhoYou,
                    action: "Dead"
                }
                GL.GameStatus = "Done";
                P2P.peer.send(JSON.stringify(data));
            } else {
                if (player.playerID == 1)
                    GL.grid[oldGridY][oldGridX] = 4;
                if (player.playerID == 2)
                    GL.grid[oldGridY][oldGridX] = 5;
                //place light player behind
                GL.PlaceLight(player, oldGridX, oldGridY);
                GL.grid[player.gridY][player.gridX] = player.playerID;
                GL.UpdateSprite(player)
                var data = {
                    player: GL.WhoYou,
                    action: "Move",
                    x: player.gridX,
                    y: player.gridY
                }
                P2P.peer.send(JSON.stringify(data));
            }
        }
    },
    UpdateSprite: function (player) {
        player.sprite.y = player.gridY * 60;
        player.sprite.x = player.gridX * 60;
        switch (player.dir) {
            case 'left':
                player.sprite.angle = 0;
                break;
            case 'up':
                player.sprite.angle = 90;
                break;
            case 'right':
                player.sprite.angle = 180;
                break;
            case 'down':
                player.sprite.angle = 270;
                break;
        }
    },
    PlaceLight: function (player, gridx, gridy) {
        var light;
        if (player.playerID == 1)
            light = game.add.sprite(gridx * 60, gridy * 60, 'player1_light');
        if (player.playerID == 2)
            light = game.add.sprite(gridx * 60, gridy * 60, 'player2_light');
        light.anchor.setTo(0.5, 0.13);
        switch (player.dir) {
            case 'up':
                light.angle = 180;
                break;
            case 'down':
                light.angle = 0;
                break;
            case 'left':
                light.angle = 90;
                break;
            case 'right':
                light.angle = 270;
                break;
            default:
                light.angle = 0;
                break;
        }
    },
    DEBUG_GRID: function () {
        for (var y = 0; y <= GL.GridMaxY ; y++) {
            var str = "";
            for (var x = 0; x <= GL.GridMaxX ; x++) {
                str += GL.grid[y][x] + " ";
            }
            console.log(str);
        }
    },
    UpdateInterID: null
}


var LFS; // looking for server interval id;
// When all ready, then it call this function
$(document).ready(function () {
    GetUserInfo(sessionStorage.getItem('user'), sessionStorage.getItem('pass'));
    locationHashChanged();
    window.onhashchange = locationHashChanged;
    GetCountries();
    $('#btn_signup').click(function () {
        var username = $('#username_signup').val();
        var password = $('#password_signup').val();
        var email = $('#email_signup').val();
        var country = $('#country_signup').val();
        SignupUser(username, password, email, country);
    });
    $('#btn_login').click(function () {
        var username = $('#username_login').val();
        var password = $('#password_login').val();
        LoginUser(username, password);
    });
    $('#btn_createserver').click(function () {
        var name = $('#name_createserver').val();
        CreateServer(name,sessionStorage.getItem('user'),P2P.peer_data);
    });
    
    InitGame();
    $(".answers_model").fadeIn("slow");
})

function locationHashChanged() {
    if(P2P.peer)
        if (P2P.peer.connected == true && location.hash !== "#Lobby") {
        console.log('Kill peer!');
        location.reload();
        } 
    /*
    if (LFA && !(location.hash === "#Lobby")) {
        location.reload();
    }
    if (LFS) {
        location.reload();
    }
    */
    if (location.hash === "#CreateServer" && sessionStorage.getItem('user')) {
        $(".se-pre-con").fadeIn("slow");
        P2P.InitPeer(true);
        var id = setInterval(function () { if (P2P.isloaded) { $(".se-pre-con").fadeOut("slow"); clearInterval(id);} }, 100);
        show('CreateServer');
    }
    else if (location.hash === "#ServerList" && sessionStorage.getItem('user')) {
        P2P.InitPeer(false);
        GetServers();
        LFS = setInterval(function () {
            GetServers();
        }, 5000);
        show('ServerList');
        $(".waiting_model").fadeOut(0);
        $(".se-pre-con").fadeOut("slow");
    }
    else if (location.hash === "#Lobby" && sessionStorage.getItem('user') && P2P.isloaded) {
        show('Lobby');
        $(".se-pre-con").fadeOut("slow");
    }
    else if (location.hash === "#SignUp" && !sessionStorage.getItem('user')) {
        show('SignUp');
        $(".se-pre-con").fadeOut("slow");
    }
    else if (location.hash === "#Login" && !sessionStorage.getItem('user')) {
        show('Login');
        $(".se-pre-con").fadeOut("slow");
    }
    else {
        location.href = "#";
        show('Home');
        
        $(".se-pre-con").fadeOut("slow");
    }
}
window.onbeforeunload = window.onunload = function () {
    removeServer(sessionStorage.getItem('user'));
}
function removeServer(hostusername) {
    $.ajax({
        type: "POST",
        url: "WebService1.asmx/DeleteServerByHosterName",
        data: '{hosterName: "' + hostusername + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
