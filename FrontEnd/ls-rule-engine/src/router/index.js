import {createRouter, createWebHistory} from 'vue-router'
import HelloWorld from '../components/HelloWorld.vue'
import RuleTesting from '../components/RuleTesting.vue'

const routes = [
    {path: '/', name: 'HelloWorld', component: HelloWorld},
    {path: '/Dashboard', name: 'Dashboard', component: HelloWorld},
    {path: '/RuleTesting', name: 'RuleTesting', component: RuleTesting},

]

const router = createRouter( {
    history: createWebHistory(),
    routes
})


export default router