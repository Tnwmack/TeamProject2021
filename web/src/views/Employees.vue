<template>
    <global-header />
    
    <div class="employees row flex-grow-1 align-content-start">
        <h1 class="pt-5">Employees</h1>
        <SearchBox @search="onSearch" placeholder="Search employees"/>
        <EmployeeList @on-Activation="onActivation" @on-add="onAdd" @on-sort="onSort" @on-page="onPage" :employees="employeesList"/>
        <div class="d-flex justify-content-end">
            <a v-if="employeesQuery && employeesQuery.offset > 0" class="p-2" href="" @click.prevent="onPage('previous')">Previous</a>
            <a v-if="employeesQuery && employeesQuery.offset + employeesQuery.employees.length < employeesQuery.total" class="p-2" href="" @click.prevent="onPage('next')">Next</a>
        </div>
    </div>

    <AddEmployeeModal v-if="showAddEmployee" @on-Close="onAddEmployeeModalClose" />
    <ConfirmationBox v-if="confirmBox" :text="confirmBox" @on-Close="onActivationConfirmation" />

    <global-footer />
</template>

<script>
// @ is an alias to /src
import GlobalHeader from "@/components/GlobalHeader.vue";
import GlobalFooter from "@/components/GlobalFooter.vue";
import EmployeeList from "@/components/EmployeeList.vue";
import SearchBox from "@/components/SearchBox.vue";
import { useToast } from "vue-toastification";
import CorpAPI from "@/api";

import AddEmployeeModal from "@/components/AddEmployeeModal.vue";
import ConfirmationBox from "@/components/ConfirmationBox.vue";

export default {
  name: 'Employees',
  components: {
    GlobalHeader,
    GlobalFooter,
    EmployeeList,
    SearchBox,
    AddEmployeeModal,
    ConfirmationBox
  },
  data() {
        return {
            employeesQuery: null,
            employeesList: null,
            confirmBox: null,
            searchTerm: "",
            sortColumn: "id",
            sortDirection: "asc",
            showAddEmployee: false
        };
    },
    setup() {
        const toast = useToast();
        return { toast };
    },
    async mounted() {
        await this.loadEmployees(0, 10);
    },
    methods: {
        async loadEmployees(offset, size) {
            let data = this;

            this.employeesQuery = null;
            this.employeesList = null;

            let filter = [];

            if (this.searchTerm && this.searchTerm.length > 0)
                filter.push(this.searchTerm);

            if(!this.$store.state.user?.admin)
                filter.push("hideinactive");

            let apiEmployees = await CorpAPI.getEmployees(offset, size, filter, this.sortColumn + "_" + this.sortDirection);
            
            if(apiEmployees) {
                setTimeout(function () {
                    data.employeesQuery = apiEmployees;
                    data.employeesList = apiEmployees.employees;
                }, 1000);
            }
            else {
                data.toast.error("Loading employees failed", {timeout: false});
            }
        },
        onActivation(id, active) {
            this.confirmBox = {
                title: active ? "Confirm Activate" : "Confirm Deactivate",
                message: `Are you sure you want to ${active ? 'activate' : 'deactivate'} employee ${id}?`,
                cancel: "No",
                accept: "Yes",
                id: id,
                active: active,
            };
        },
        async onActivationConfirmation(accepted) {
            if(accepted) {
                const id = this.confirmBox.id;
                const active = this.confirmBox.active;
                this.confirmBox = null;

                let updatedEmployee = this.employeesList.find(e => e.id === id);
                updatedEmployee.active = active;

                if(await CorpAPI.updateEmployee(updatedEmployee)) {
                    this.toast.success(`Employee ${id} ${active ? 'activated' : 'deactivated'}`);
                }
                else {
                    this.toast.error(`${active ? 'Activate' : 'Deactivate'} employee failed`, {timeout: false});
                }
            }
            else {
                this.confirmBox = null;
            }
        },
        async onAdd() {
            this.showAddEmployee = true;
        },
        async onAddEmployeeModalClose(success) {
            this.showAddEmployee = false;

            if(success) {
                await this.loadEmployees(this.employeesQuery.offset, 10);
            }
        },
        async onSearch(term) {
            this.searchTerm = term;
            this.sortColumn = "id";
            this.sortDirection = "asc";
            await this.loadEmployees(0, 10);
        },
        async onSort(column) {
            if(column === this.sortColumn) {
                if(this.sortDirection === "asc")
                    this.sortDirection = "dec";
                else
                    this.sortDirection = "asc";
            }
            else {
                this.sortColumn = column;

                if(this.sortColumn === "hiredate")
                    this.sortDirection = "dec";
                else
                    this.sortDirection = "asc";
            }

            await this.loadEmployees(0, 10);
        },
        async onPage(direction) {
            if(direction === "next") {
                await this.loadEmployees(this.employeesQuery.offset + this.employeesQuery.employees.length, 10);
            }
            else if(direction === "previous") {
                if(this.employeesQuery.offset - 10 >= 0)
                    await this.loadEmployees(this.employeesQuery.offset - 10, 10);
                else
                    await this.loadEmployees(0, 10);
            }
        }
    },
    computed: {
        user() {
            return this.$store.state.user;
        }
    },
    watch: {
        async user() {
            await this.loadEmployees(0, 10);
        }
    }
};
</script>

<style scoped lang="scss">

</style>
