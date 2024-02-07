import {createRouter, createWebHistory} from 'vue-router'
import HelloWorld from '../components/HelloWorld.vue'
import RuleTesting from '../components/RuleTesting.vue'
import RuleManagement from '../components/RuleManagement.vue'

const routes = [
    {path: '/', name: 'HelloWorld', component: HelloWorld},
    {path: '/home', name: 'HelloWorld', component: HelloWorld},
    {path: '/Dashboard', name: 'Dashboard', component: HelloWorld},
    {path: '/RuleTesting', name: 'RuleTesting', component: RuleTesting},
    {path: '/rule-management', name: 'RuleManagement', component: RuleManagement},

]

const router = createRouter( {
    history: createWebHistory(),
    routes
})


export default router