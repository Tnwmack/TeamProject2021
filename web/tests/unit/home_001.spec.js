import { mount } from '@vue/test-utils'
import App from '@/App.vue'
import router from '@/router'

test('Page should display Corpsite name', async () => {
  router.push('/')

  // After this line, router is ready
  await router.isReady()

  const wrapper = mount(App, {
    global: {
      plugins: [router]
    }
  })
  expect(wrapper.html()).toContain('Corpsite')
})

test('Page should have a Products link', async () => {
  router.push('/')

  // After this line, router is ready
  await router.isReady()

  const wrapper = mount(App, {
    global: {
      plugins: [router]
    }
  })
  expect(wrapper.html()).toContain('<a href="/products"')
  expect(wrapper.html()).toContain('>Products<')
})

test('Page should have a About Us link', async () => {
  router.push('/')

  // After this line, router is ready
  await router.isReady()

  const wrapper = mount(App, {
    global: {
      plugins: [router]
    }
  })
  expect(wrapper.html()).toContain('<a href="/about"')
  expect(wrapper.html()).toContain('>About Us<')
})

test('Page should have a Employees link', async () => {
  router.push('/')

  // After this line, router is ready
  await router.isReady()

  const wrapper = mount(App, {
    global: {
      plugins: [router]
    }
  })
  expect(wrapper.html()).toContain('<a href="/employees"')
  expect(wrapper.html()).toContain('>Our Team<')
})

test('Page should have a Home link via site brand area', async () => {
  router.push('/')

  // After this line, router is ready
  await router.isReady()

  const wrapper = mount(App, {
    global: {
      plugins: [router]
    }
  })
  expect(wrapper.html()).toContain('href="/"')
  expect(wrapper.html()).toContain('> Corpsite</a>')
})
