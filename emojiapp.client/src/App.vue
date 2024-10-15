<template>
    <div class="container">
        <div class="row">
            <div v-for="emoji in emojis" :key="emoji.name" class="col-md-3">
                <div class="card text-center">
                    <div class="card-body">
                        <span class="display-1">{{ emoji.name }}</span>
                        <button class="btn btn-outline-danger mt-3" @click="likeEmoji(emoji.name)">
                            ❤️ {{ emoji.likes }}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import * as signalR from '@microsoft/signalr';

    // Initial emoji list
    const emojis = ref([
        { name: '😊', likes: 0 },
        { name: '😂', likes: 0 },
        { name: '❤️', likes: 0 },
        { name: '👍', likes: 0 }
    ]);

    let startedPromise = null
    let manuallyClosed = false
    let connection;

    onMounted(() => {


        // Receives the initial list of emojis and their likes
        connection.on('ReceiveInitialLikes', (initialEmojis) => {
            emojis.value = initialEmojis;
        });

        // Receives an updated emoji object
        connection.on('UpdateEmoji', (updatedEmoji) => {
            const emoji = emojis.value.find(e => e.name === updatedEmoji.name);
            if (emoji) {
                emoji.likes = updatedEmoji.likes;
            }
        });

        // Connexion standard
        // connection.start().catch(err => console.error(err));

        // Auto-reconnexion en cas de déconnexion par le serveur...
        connection.onclose(() => {
            if (!manuallyClosed) start()
        })

    });

    const likeEmoji = async (emojiName) => {
        if (connection) {
            await connection.invoke('LikeEmoji', emojiName);
        }
    };

    function start() {
        //var url = "https://localhost:7222/emojiHub"
        //var url = "https://sqlinfocg.cegepgranby.qc.ca/cboucher"
        var endpoint = "/emojiHub"
        var url = `${import.meta.env.VITE_BASE_URL}${import.meta.env.BASE_URL}${endpoint}`
        console.log("VITE base url: " + import.meta.env.VITE_BASE_URL)
        console.log("Base url: " + import.meta.env.BASE_URL)
        console.log("Endpoint: " + endpoint)
        console.log("Complete URL: " + url)

        connection = new signalR.HubConnectionBuilder()
            .withUrl(url)
            .build();

        startedPromise = connection.start()
            .catch(err => {
                console.error('Failed to connect with hub', err)
                return new Promise((resolve, reject) => setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
        return startedPromise
    }

    // Tentative de connexion avec reconnexion
    manuallyClosed = false
    start()
</script>

<style>
    .container {
        margin-top: 50px;
    }

    .display-1 {
        font-size: 5rem;
    }
</style>
