import { fileURLToPath, URL } from 'node:url';

import { defineConfig, loadEnv } from 'vite'

import plugin from '@vitejs/plugin-vue';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';
import vue from "@vitejs/plugin-vue";


//const baseFolder =
//    env.APPDATA !== undefined && env.APPDATA !== ''
//        ? `${env.APPDATA}/ASP.NET/https`
//        : `${env.HOME}/.aspnet/https`;

//const certificateName = "emojiapp.client";
//const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
//const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

//if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
//    if (0 !== child_process.spawnSync('dotnet', [
//        'dev-certs',
//        'https',
//        '--export-path',
//        certFilePath,
//        '--format',
//        'Pem',
//        '--no-password',
//    ], { stdio: 'inherit', }).status) {
//        throw new Error("Could not create certificate.");
//    }
//}

//const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
//    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7222';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {


    // Load env file based on `mode` in the current working directory.
    // Set the third parameter to '' to load all env regardless of the `VITE_` prefix.
    const env = loadEnv(mode, process.cwd(), '');
    //console.log(env)
    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        //server: {
        //    proxy: {
        //        '^/weatherforecast': {
        //            target,
        //            secure: false
        //        }
        //    },
        //    port: 5173,
        //    https: {
        //        key: fs.readFileSync(keyFilePath),
        //        cert: fs.readFileSync(certFilePath),
        //    }
        //},
        base: env.NODE_ENV == "production" ? "/cboucher" : "/",
        build: {
            sourcemap: true
        }
    }
})
