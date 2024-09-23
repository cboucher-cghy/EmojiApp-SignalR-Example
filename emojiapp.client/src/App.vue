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

    let connection;

    onMounted(() => {
        connection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7222/emojiHub')
            .build();

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

        connection.start().catch(err => console.error(err));
    });

    const likeEmoji = async (emojiName) => {
        if (connection) {
            await connection.invoke('LikeEmoji', emojiName);
        }
    };
</script>

<style>
    .container {
        margin-top: 50px;
    }

    .display-1 {
        font-size: 5rem;
    }
</style>
