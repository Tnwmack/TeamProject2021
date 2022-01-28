<template>
    <div id="modalBox" class="modal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{product ? "Edit a product" : "Add a product"}}</h5>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr><td style="text-align: right;">{{product ? "Replace Current Image:" : "Image:"}}</td><td><input class="form-control" type="file" @change="onImageFileSelected" ref="inputImageFile"></td></tr>
                        <tr v-if="showImageUploading"><td></td><td><div class="alert alert-warning table-warning">Image Uploading...</div></td></tr>
                        <tr v-if="imageErrorMessage"><td></td><td><div class="alert alert-danger table-danger">{{imageErrorMessage}}</div></td></tr>

                        <tr><td style="text-align: right;">Name:</td><td><input class="form-control" v-model="name" placeholder="enter product name"></td></tr>
                        <tr v-if="nameError"><td></td><td><div class="alert alert-danger table-danger">Name required</div></td></tr>
                        <tr>
                            <td style="text-align: right;">Type:</td>
                            <td>
                                <select v-model="type" class="form-select">
                                    <option value="Appliances">Appliances</option>
                                    <option value="Cleaners">Cleaners</option>
                                    <option value="Home Decor">Home Decor</option>
                                    <option value="Pets">Pets</option>
                                    <option value="Tools">Tools</option>
                                </select>
                            </td>
                        </tr>
                        <tr v-if="typeError"><td></td><td><div class="alert alert-danger table-danger">Type required</div></td></tr>
                        <tr><td style="text-align: right;">Quantity:</td><td><input class="form-control" type="number" v-model="quantity" placeholder="enter quantity"></td></tr>
                        <tr><td style="text-align: right;">Description:</td><td><textarea class="form-control" v-model="description" placeholder="enter description"></textarea></td></tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="onCloseCancel()">Cancel</button>
                    
                    <button v-if="showWorkingButton" type="button" class="btn btn-warning btn-working">Sending...</button>
                    <button v-else type="button" class="btn btn-primary" @click="onAccept()">Accept</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import bootstrap from "bootstrap/dist/js/bootstrap";
import { useToast } from "vue-toastification";
import CorpAPI from "@/api";

export default {
    name: "AddProductModal",
    data() {
        return {
            showWorkingButton: false,
            showImageUploading: false,
            box: null,
            newProductImage: null,
            name: "",
            type: "Appliances",
            quantity: 0,
            description: "",
            nameError: false,
            typeError: false,
            imageErrorMessage: null,
            manualClose: false,
        };
    },
    props: {
        product: Object
    },
    setup() {
        const toast = useToast();
        return { toast };
    },
    mounted() {
        let data = this;

        if(this.product) {
            this.name = this.product.name;
            this.type = this.product.type;
            this.quantity = this.product.quantity;
            this.description = this.product.description;
        }

        let boxDom = document.getElementById("modalBox");
        this.box = new bootstrap.Modal(boxDom);

        boxDom.addEventListener('hidden.bs.modal', function () {
            data.onModalClosed();
        });

        this.box.show();
    },
    methods: {
        onImageFileSelected(event) {
            this.imageErrorMessage = null;
            let target = event.target;
            if (target.files[0].size/1024 > 300) {
                this.$refs.inputImageFile.value = null;
                this.imageErrorMessage = "Product image size cannot exceed 300kb";
            }
            if (target.files[0].type != "image/png") {
                this.$refs.inputImageFile.value = null;
                this.imageErrorMessage = "Only .png image files are supported.";
            }
            this.newProductImage = target.files[0];
        },
        validateForm() {
            let validated = true;

            if(this.name.trim().length === 0) {
                validated = false;
                this.nameError = true;
            } else {
                this.nameError = false;
            }

            if(this.type.trim().length === 0) {
                validated = false;
                this.typeError = true;
            } else {
                this.typeError = false;
            }

            return validated;
        },
        async onAccept() {
            if(this.validateForm()) {
                this.showWorkingButton = true;

                if(await this.submitProduct()) {
                    this.toast.success(this.product ? "Product updated" : "Product added");
                    this.onCloseSuccess();
                }
                else {
                    this.toast.error(this.product ? "Update product failed" : "Add product failed", {timeout: false});
                }

                this.showWorkingButton = false;
            }
        },
        onCloseCancel() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', false);
        },
        onCloseSuccess() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', true);
        },
        onModalClosed() {
            if(this.manualClose === false)
                this.$emit('onClose', false);
        },
        async submitProduct() {
            let imageLink = await this.submitImage();

            if(imageLink === null)
                return false;
            
            const newProduct = {
                'name': this.name,
                'type': this.type,
                'quantity': this.quantity,
                'description': this.description.trim().length === 0 ? undefined : this.description,
                'imageLink': imageLink ? imageLink : this.product?.imageLink,
            };

            if(this.product) {
                newProduct.id = this.product.id;
                return await CorpAPI.updateProduct(newProduct);
            }
            else {
                return await CorpAPI.createProduct(newProduct);
            }
        },
        async submitImage() {
            let imageLink = undefined;

            if (this.newProductImage) {
                this.showImageUploading = true;

                // Using form data to enacapsulate image for axios.post
                const newProductImage = new FormData();
                newProductImage.append('productImage', this.newProductImage, "na.png");
            
                imageLink = await CorpAPI.uploadProductImage(newProductImage)
            
                if (!imageLink) {
                    this.toast.error("Upload product image failed", {timeout: false});
                    this.imageErrorMessage = "Upload failed";
                    return null;
                }

                this.showImageUploading = false;
            }

            return imageLink;
        }
    },
    emits: ['onClose']
};
</script>

<style scoped lang="scss">
    .btn-working {
        cursor: progress;
    }

</style>