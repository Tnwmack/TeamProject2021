import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Employees from '../views/Employees.vue'
import Products from '../views/Products.vue'
import ProductDetails from '../views/ProductDetails.vue'
import EmployeeDetails from '../views/EmployeeDetails.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/products',
    name: 'Products',
    component: Products
  },
  {
    path: '/employees',
    name: 'Employees',
    component: Employees
    },
  {
    path: '/productdetails/:id',
    name: 'ProductDetails',
    component: ProductDetails

  },
  {
    path: '/employeedetails/:id',
    name: 'EmployeeDetails',
    component: EmployeeDetails

  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
