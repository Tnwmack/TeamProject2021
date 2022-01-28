<template>
    <global-header />

    <h1 class="pt-5">Employee Details</h1>

    <div class="about row flex-grow-1 align-content-left">
        <div><img :src="gravatar(employee?.email)" height="200" class="d-inline-block"></div>
        <div>Name: {{name}}</div>
        <div>Title: {{employee?.title}}</div>
        <div>Id: {{employee?.id}}</div>
        <div>Tenure: {{tenure(employee?.hireDate)}}</div>
        <div>Email: {{employee?.email}}</div>
        <div v-if="user?.admin">Status: {{employee?.active? "Active":"Deactivated"}}</div>
        <div>Bio: {{employee?.bio}}</div>
    </div>
    
    <th><button v-if="showEdit" type="button" class="btn btn-m btn-primary py-1" title="Edit" @click="onEdit()">Edit</button></th>

    <AddEmployeeModal v-if="showEditEmployee" @on-Close="onAddEmployeeModalClose" :employee="employee" />

    <global-footer />
</template>

<script>
import GlobalHeader from '@/components/GlobalHeader.vue'
import GlobalFooter from '@/components/GlobalFooter.vue'
import AddEmployeeModal from '@/components/AddEmployeeModal.vue'
import { useToast } from "vue-toastification";
import md5 from "md5";
import CorpAPI from "@/api";
//import VueRouter from 'vue-router';


    export default {
        name: 'EmployeeDetails',
        components: {
            GlobalHeader,
            GlobalFooter,
            AddEmployeeModal
        },
        data() {
            return {
                employeeId: null,
                hireDate: null,
                email: null,
                bio: null,
                title: null,
                active: null,
                showEditEmployee: false,
                employee: null
            };
        },
        setup() {
            const toast = useToast();
            return { toast };
        },
        computed: {
            showEdit() {
                if(!this.employee)
                    return false;

                if(this.user?.admin)
                    return true;

                let username = this.employee.firstName.toLowerCase().charAt(0) +
                    this.employee.lastName.toLowerCase();

                if(username === this.user?.name)
                    return true;

                return false;
            },
            user() {
                return this.$store.state.user;
            },
            name() {
                return this.employee?.firstName + " " + this.employee?.lastName
            }
        },
        async mounted() {
            await this.loadDetails(this.$route.params.id);
        },
        methods: {
            async loadDetails(id) {
                this.employee = await CorpAPI.getEmployee(id);
                if (this.employee) {
                    if (!this.employee.active && !this.user?.admin) {
                        this.employee = null;
                        this.toast.error("Selected employee is no longer active.", { timeout: false });
                    }
                }
                else {
                    this.toast.error("Loading employee failed", { timeout: false });
                }
            },
            onEdit() {
                if (this.employee) {
                    this.showEditEmployee = true;
                } else {
                    this.toast.error("Cannot edit this employee.", { timeout: false });
                }
            },
            async onAddEmployeeModalClose(success) {
                this.showEditEmployee = false;

                if(success)
                    await this.loadDetails(this.$route.params.id);
            },
            gravatar(email) {
                if (email) {
                    const hash = md5(email.trim().toLowerCase());
                    return `https://www.gravatar.com/avatar/${hash}?d=mp&s=200`;
                }
                return '/employees/na_employee.png'
            },
            tenure(hireDate) {
                if (!hireDate) return '';
                let tenureMillis = Date.now() - Date.parse(hireDate); //Date.now() - hireDate;
                let tenureAsDate = new Date(tenureMillis); // as in, miliseconds from epoch
                let tenureInYears = Math.abs(tenureAsDate.getFullYear() - 1970);
                return (tenureInYears < 1? '<1' : tenureInYears) + (tenureInYears <= 1? ' year' : ' years');
            }
        }
    }
</script>